/****************************************************
  文件：PathInfo.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-12 21:26:55
  功能：
*****************************************************/

using YooAsset;

namespace Zero.Utility
{
    public class PatchInfo
    {
        private EPlayMode _playMode;
        private string _packageName;
        private string _buildPipeline;
        private DecryptionType _decryptionType;
        private bool _isSetDefault;
        private string _remoteIP;
        private string _appVersion;
        
        public PatchInfo(EPlayMode playMode, 
            string packageName = "DefaultPackage", string buildPipeline = null,
            DecryptionType decryptionType = DecryptionType.None, bool isSetDefault = true, 
            string remoteIP = "", string appVersion = "")
        {
            _playMode = playMode;
            _packageName = packageName;
            _buildPipeline = buildPipeline;
            _decryptionType = decryptionType;
            _isSetDefault = isSetDefault;
            _remoteIP = remoteIP;
            _appVersion = appVersion;
        }

        public EPlayMode playMode
        {
            get => _playMode;
            set => _playMode = value;
        }

        public string packageName
        {
            get => _packageName;
            set => _packageName = value;
        }

        public string buildPipeline
        {
            get => _buildPipeline;
            set => _buildPipeline = value;
        }

        public DecryptionType decryptionType
        {
            get => _decryptionType;
            set => _decryptionType = value;
        }

        public bool isSetDefault
        {
            get => _isSetDefault;
            set => _isSetDefault = value;
        }

        public string remoteIP
        {
            get => _remoteIP;
            set => _remoteIP = value;
        }

        public string appVersion
        {
            get => _appVersion;
            set => _appVersion = value;
        }
    }
}