/****************************************************
  文件：PoolExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/23 20:45:06
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Samples
{
    public class PoolExample : MonoBehaviour
    {
        public GameObject prefab;

        public int ZeroCountActive;
        public int ZeroCountAll;
        public int ZeroCountInactive;

        public int UnityCountActive;
        public int UnityCountAll;
        public int UnityCountInactive;
        
        // 实际使用时二选其一即可
        private IZeroObjectPool<GameObject> _zeroPool;
        private UnityEngine.Pool.ObjectPool<GameObject> _unityPool;

        private Queue<GameObject> _zeroQueue = new Queue<GameObject>();
        private Queue<GameObject> _unityQueue = new Queue<GameObject>();

        public float zero_x = -10;
        public float unity_x = -10;
        // Start is called before the first frame update
        void Start()
        {
            //1.使用ZeroFramework提供的对象池: SimpleObjectPool
            _zeroPool = ZeroToolKits.Instance.Pool.AllocateSimpleObjectPool<GameObject>(
                CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy,false, 3, 5, false);
            //2.使用ZeroFramework提供的对象池: SafeObjectPool
            // _zeroPool = this.GetZeroToolKits().PoolKit.AllocateSafeObjectPool<GameObject>(
            //     CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy,false, 5);
            
            //3.使用Unity提供的对象池（2021.3之后支持）
            _unityPool = new UnityEngine.Pool.ObjectPool<GameObject>(
                CreateFuncUnity, ActionOnGet, ActionOnRelease, ActionOnDestroy, false, 3, 5);
        }

        // Update is called once per frame
        void Update()
        {
            ZeroCountActive = _zeroPool.GetCountActive();
            ZeroCountAll = _zeroPool.GetCountAll();
            ZeroCountInactive = _zeroPool.GetCountInactive();
            
            UnityCountActive = _unityPool.CountActive;
            UnityCountAll = _unityPool.CountAll;
            UnityCountInactive = _unityPool.CountInactive;

            #if ENABLE_INPUT_SYSTEM
            #else
                if (Input.GetMouseButtonDown(0))
                {
                    _zeroQueue.Enqueue(_zeroPool.Get());
                    _unityQueue.Enqueue(_unityPool.Get());
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (_zeroQueue.Count > 0)
                    {
                        _zeroPool.Release(_zeroQueue.Dequeue());
                        _unityPool.Release(_unityQueue.Dequeue());
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    _zeroPool.Clear();
                    _unityPool.Clear();
                }
                if (Input.GetKeyDown(KeyCode.W)) //重复放回（非法！）
                {
                    _zeroPool.Release(_zeroQueue.Peek());
                    _unityPool.Release(_unityQueue.Peek());
                }
            #endif
        }

        public GameObject CreateFunc()
        {
            var obj = Instantiate(prefab, Vector3.right * zero_x, Quaternion.identity);
            obj.name = $"ZeroPoolObj:{zero_x}";
            zero_x += 2;
            obj.SetActive(false);
            return obj;
        }
        
        public GameObject CreateFuncUnity()
        {
            var obj = Instantiate(prefab, Vector3.right * unity_x + Vector3.up * 2, Quaternion.identity);
            obj.name = $"UnityPoolObj:{unity_x}";
            unity_x += 2;
            obj.SetActive(false);
            return obj;
        }

        public void ActionOnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        public void ActionOnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        public void ActionOnDestroy(GameObject obj)
        {
            Destroy(obj);
            zero_x -= 2 * 0.5f;
            unity_x -= 2 * 0.5f;
        }
    }
}