/****************************************************
  文件：FileExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/25 22:37:44
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zero.Utility;

namespace Zero.Samples
{
    public class FileExample : MonoBehaviour
    {
        [Serializable]
        public class FileObjExample
        {
            public string name = "congtou";
            public float pi = 3.14f;
            public int age = 24;

            public string[] array = new[]
            {
                "Unity", "2021.3.x", "C#"
            };
        
            public List<string> list = new List<string>()
            {
                "Unity", "2021.3.x", "C#"
            };
        
            public Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "Platform", "Unity" },
                { "Version", "2021.3.x" },
                { "Language", "C#" },
            };
        }
        
        public Image image_web;
        public Image image_local;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            // 获取文件工具包
            var fileKit = ZeroToolKits.Instance.File;
            //生成测试对象
            FileObjExample obj = new FileObjExample();
            //序列化对象
            string json = fileKit.JsonTool.SerializeObject(obj);
            string yaml = fileKit.YamlTool.SerializeObject(obj);
            Debug.Log("json: " + json);
            Debug.Log("yaml: " + yaml);
            
            //写入文本 
            string json_path = Path.Combine(Application.dataPath, "ZeroFramework/Samples/Utility/File/Example/test_json.json");;
            fileKit.TextTool.Write(json_path, json);
            string yaml_path = Path.Combine(Application.dataPath, "ZeroFramework/Samples/Utility/File/Example/test_yaml.yaml");
            fileKit.TextTool.WriteAsync(yaml_path, yaml, null);
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif

            yield return new WaitForSeconds(2.0f);
            
            //读取文本
            string json2 = fileKit.TextTool.ReadFromUri("file://" + json_path);
            Debug.Log("读取json: " + json2);
            var json_obj = fileKit.JsonTool.DeserializeObject<FileObjExample>(json2);
            fileKit.TextTool.ReadAsync(yaml_path, yaml2 =>
            {
                Debug.Log("异步读取yaml: " + yaml2);
                var yaml_obj = fileKit.YamlTool.DeserializeObject<FileObjExample>(yaml2);
                Debug.Log("Compare Name: " + (json_obj.name == yaml_obj.name));
            });
            Debug.Log("Local File Example Over!");
            
            //从网络上读取文本
            string uri = "https://www.baidu.com/index.html";
            yield return fileKit.TextTool.ReadFromUri(uri, content =>
            {
                if (!string.IsNullOrEmpty(content))
                {
                    Debug.Log("成功读取uri内容：" + content);
                }
                else
                {
                    Debug.Log("读取内容失败，请检查网络和uri: " + uri);
                }
                
            });
            
            //从网络上下载图片
            string imageUri_web =
                "http://mms0.baidu.com/it/u=1954923991,1429701627&fm=253&app=120&f=JPEG?w=800&h=500";
            yield return fileKit.ImageTool.ReadFromUri(imageUri_web, texture2D =>
            {
                if (texture2D)
                {
                    Debug.Log("成功读取uri内容：" + texture2D.name);
                    image_web.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                }
                else
                {
                    Debug.Log("读取内容失败，请检查网络和uri: " + imageUri_web);
                }
            });
            
            string imageUri_local =
                Path.Combine(Application.dataPath, "ZeroFramework/Samples/Utility/File/Example/Hollow.jpg");
            yield return fileKit.ImageTool.ReadFromUri(imageUri_local, sprite =>
            {
                if (sprite != null)
                {
                    Debug.Log("成功读取uri内容：" + sprite.name);
                    image_local.sprite = sprite;
                }
                else
                {
                    Debug.Log("读取内容失败，请检查网络和uri: " + imageUri_local);
                }
            });
        }
    }
}
