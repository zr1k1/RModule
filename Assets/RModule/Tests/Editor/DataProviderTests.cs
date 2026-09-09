using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

public class DataProviderTests {
    enum TestValue {
        Number = 0,
        Text = 1,
        Dictionary = 2
    }

    [Test]
    public void GetValue_Int_ReturnsValueDirectly() {
        var values = new Dictionary<int, object> {
            [(int)TestValue.Number] = 123
        };

        var data = new Data<TestValue>(values);

        var result = data.GetValue<int>(TestValue.Number);

        Assert.AreEqual(123, result);
    }

    [Test]
    public void GetValue_String_ReturnsValueDirectly() {
        var values = new Dictionary<int, object> {
            [(int)TestValue.Text] = "Hello"
        };

        var data = new Data<TestValue>(values);

        var result = data.GetValue<string>(TestValue.Text);

        Assert.AreEqual("Hello", result);
    }

    [Test]
    public void GetValue_SerializableDictionary_FromJObject_ReturnsCorrectType() {
        var dictionary = new SerializableDictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        // Имитируем то, что происходит после:
        //
        // JsonConvert.DeserializeObject<JData>()
        //
        // когда Dictionary<int, object> содержит сложный объект.
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(dictionary);
        var jObject = JObject.Parse(json);

        var values = new Dictionary<int, object> {
            [(int)TestValue.Dictionary] = jObject
        };

        var data = new Data<TestValue>(values);

        var result = data.GetValue<SerializableDictionary<string, int>>(
            TestValue.Dictionary);

        Assert.IsNotNull(result);

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(1, result["one"]);
        Assert.AreEqual(2, result["two"]);
        Assert.AreEqual(3, result["three"]);
    }

    [Test]
    public void GetValue_SerializableDictionary_FromJObject_ReturnsNewObject() {
        var dictionary = new SerializableDictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 }
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(dictionary);
        var jObject = JObject.Parse(json);

        var values = new Dictionary<int, object> {
            [(int)TestValue.Dictionary] = jObject
        };

        var data = new Data<TestValue>(values);

        var result1 = data.GetValue<SerializableDictionary<string, int>>(
            TestValue.Dictionary);

        var result2 = data.GetValue<SerializableDictionary<string, int>>(
            TestValue.Dictionary);

        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);

        Assert.AreNotSame(result1, result2);

        Assert.AreEqual(result1["one"], result2["one"]);
        Assert.AreEqual(result1["two"], result2["two"]);
    }
}
