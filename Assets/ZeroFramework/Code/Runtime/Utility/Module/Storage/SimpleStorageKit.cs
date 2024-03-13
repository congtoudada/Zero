/****************************************************
  文件：SimpleStorageKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 10:50:15
  功能：
*****************************************************/

namespace Zero.Utility
{
    public class SimpleStorageKit : ISimpleStorageKit
    {
        private SimpleStorageInfo _info;
        private ISimpleStorageHelper _helper;

        public SimpleStorageKit(SimpleStorageInfo.StorageMethod method)
        {
            _info = CreateStorageInfo(method);
            EquipOverride(_info);
        }

        public SimpleStorageInfo CreateStorageInfo(SimpleStorageInfo.StorageMethod method)
        {
            return new SimpleStorageInfo(method);
        }
        
        public ISimpleStorageKit EquipOverride(SimpleStorageInfo info)
        {
            _info = info;
            switch (_info.method)
            {
                case SimpleStorageInfo.StorageMethod.PLAYER_PREFS:
                    _helper = new PlayerPrefsStorageHelper();
                    break;
                //追加新存储类...
            }
            return this;
        }

        public void SaveInt(string key, int value)
        {
            _helper.SaveInt(key, value);
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            return _helper.LoadInt(key, defaultValue);
        }

        public void SaveFloat(string key, float value)
        {
            _helper.SaveFloat(key, value);
        }

        public float LoadFloat(string key, float defaultValue = 0)
        {
            return _helper.LoadFloat(key, defaultValue);
        }

        public void SaveString(string key, string value)
        {
            _helper.SaveString(key, value);
        }

        public string LoadString(string key, string defaultValue = "")
        {
            return _helper.LoadString(key, defaultValue);
        }

        public void DeleteKey(string key)
        {
            _helper.DeleteKey(key);
        }

        public void DeleteAll()
        {
            _helper.DeleteAll();
        }
    }
}