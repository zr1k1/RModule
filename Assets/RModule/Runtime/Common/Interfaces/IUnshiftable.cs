using UnityEngine;


public interface INotUnshiftableSkiper {

}

public interface IUnshiftable {
    void SetUnshiftable(bool unshiftable);
    bool IsUnshiftable();
}

public interface IUnskipable {
}

public interface ISkipable {
    bool CanSkip();
    void SetSkipable(bool skipable);
}
