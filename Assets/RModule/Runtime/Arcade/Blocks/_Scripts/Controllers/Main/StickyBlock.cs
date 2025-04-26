namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	public class StickyBlock : BaseBlock, IAttachableSurface, IHeroSquasher, IGround {

		// Outlets
		[SerializeField] List<AttachObjectData> _attachObjectDatas = default;

		// Privats
		List<Vector2> _startMoveBlockPostitions = new List<Vector2>();
		List<Vector2> _startMoveAttachedPostitions = new List<Vector2>();
		BlockMover _blockMover;
		BlockDestroyer _blockDestroyer;
		BlockRotater _blockRotater;
		bool _isMove;
		bool _isRotate;
		bool _canAttachObjects = true;

		public event Action DidDestroyed = default;

		[Serializable]
		public class AttachObjectData {
			public GameObject Go;
			public Transform Parent;
		}

		protected override void Start() {
			_blockMover = GetComponent<BlockMover>();
			if (_blockMover != null)
				_isMove = true;
			_blockDestroyer = GetComponent<BlockDestroyer>();
			_blockRotater = GetComponent<BlockRotater>();
			if (_blockRotater != null) {
				_isRotate = true;
			}
		}

		void FixedUpdate() {
			if (_isMove) {
				for (int i = 0; i < _attachObjectDatas.Count; i++)
					_attachObjectDatas[i].Go.transform.position = _startMoveAttachedPostitions[i] + (Vector2)_blockMover.TransformToMove.position - _startMoveBlockPostitions[i];
			}
		}

		public void Attach(GameObject go) {
			Debug.Log($"StickyBlock : Attach {go.name}");
			_attachObjectDatas.Add(new AttachObjectData {
				Go = go,
				Parent = go.transform.parent
			});

			_startMoveAttachedPostitions.Add(go.transform.position);
			if (_isMove)
				_startMoveBlockPostitions.Add(_blockMover.TransformToMove.position);

			var iBlockDestroyer = go.GetComponent<IBlockDestroyer>();
			if (iBlockDestroyer != null && _blockDestroyer != null) {
				iBlockDestroyer.BlockWillDestroy(this);
				_blockDestroyer.StartDestroyAnimation(OnEndDestroy);
			}

			if (_isRotate) {
				for (int i = 0; i < _attachObjectDatas.Count; i++)
					_blockRotater.ChangeParentForRotate(_attachObjectDatas[i].Go);
			}
		}

		public void Detach(GameObject go) {
			Debug.Log($"StickyBlock : Detach");
			var attachObjectData = _attachObjectDatas.Find(data => data.Go == go);
			go.transform.SetParent(attachObjectData.Parent);
			int index = _attachObjectDatas.IndexOf(attachObjectData);
			_attachObjectDatas.RemoveAt(index);
			if (_isMove)
				_startMoveBlockPostitions.RemoveAt(index);
			_startMoveAttachedPostitions.RemoveAt(index);
		}

		void OnEndDestroy() {
			DidDestroyed?.Invoke();
		}

		public bool CanBeAttached(GameObject go) {
			return _canAttachObjects;
		}

		void OnTriggerEnter2D(Collider2D collision) {
			var iGroundCollisionHandler = collision.gameObject.GetComponent<IGroundCollisionHandler>();
			if (iGroundCollisionHandler != null && !_isRotate) {
				iGroundCollisionHandler.OnGroundEnter(gameObject);
			}
		}

		void OnTriggerExit2D(Collider2D collision) {
			var iGroundCollisionHandler = collision.gameObject.GetComponent<IGroundCollisionHandler>();
			if (iGroundCollisionHandler != null && !_isRotate) {
				iGroundCollisionHandler.OnGroundExit(gameObject);
			}
		}
	}
}