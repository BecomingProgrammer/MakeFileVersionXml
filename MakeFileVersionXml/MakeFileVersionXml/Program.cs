using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace MakeFileVersionXml
{
    class Program
    {
        private XmlDocument xmlDoc;
        //入口函数
        static void Main(string[] args)
        {
            Program app = new Program();
            string xmlName = "release1.0.0.xml";
            app.CreateXml(xmlName,"1.0");
        }


        //创建文件版本配置xml文件
        //@para:fileName:xml文件名称
        //@para version:软件版本号
        public void  CreateXml(string xmlName,string version)
        {
            try
            {
                xmlDoc = new XmlDocument();
                //写入xml版本信息
                xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration(version, null, null));
                //创建root根结点
                var root = xmlDoc.CreateElement("Update");
                xmlDoc.AppendChild(root);
                //创建软件更新地址结点
                var updateUrl = xmlDoc.CreateElement("UpdateUrl");
                //创建该结点的属性
                var attrUrl = xmlDoc.CreateAttribute("Url");
                attrUrl.Value = "软件更新地址";
                updateUrl.Attributes.Append(attrUrl);
                root.AppendChild(updateUrl);

                //软件更新时间
                var updateTime = xmlDoc.CreateElement("UpdateTime");
                var attrDate = xmlDoc.CreateAttribute("Date");
                attrDate.Value = "软件更新时间";
                updateTime.Attributes.Append(attrDate);
                root.AppendChild(updateTime);
        
                //软件更新版本号
                var updateVersion = xmlDoc.CreateElement("UpdateVersion");
                var attrVersion = xmlDoc.CreateAttribute("Version");
                attrVersion.Value = "软件更新版本号";
                updateVersion.Attributes.Append(attrVersion);
                root.AppendChild(updateVersion);

                //软件更新文件列表
                var updateFileList = xmlDoc.CreateElement("UpdateFileList");
                root.AppendChild(updateFileList);
                getFile(@"E:\暑假项目\Test\MakeFileVersionXml\MakeFileVersionXml\", updateFileList, @"MakeFileVersionXml\");

                //允许重新自启动应用
                var appRestartAllow = xmlDoc.CreateElement("AppRestartAllow");
                var attrAllow = xmlDoc.CreateAttribute("Allow");
                attrAllow.Value = "Yes";
                appRestartAllow.Attributes.Append(attrAllow);
                root.AppendChild(appRestartAllow);

                //自启动应用的名字
                var appName = xmlDoc.CreateElement("AppName");
                var attrName = xmlDoc.CreateAttribute("Name");
                attrName.Value = "软件重新启动应用的名字";
                appName.Attributes.Append(attrName);
                root.AppendChild(appName);


                xmlDoc.Save(xmlName);



            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            
        }

        //@para dir:要遍历的目录
        //@para updateFileList:xml的父结点
        //@para currentDir:相对目录
        public void getFile(string dir, XmlElement updateFileList,string currentDir)
        {
      
            try
            {
                //根目录
                var root = new DirectoryInfo(@dir);
                //获取该目录下的所有文件
                var files = root.GetFiles();
                foreach (var file in files)
                {
                    //获取文件具体信息，并写入xml
                    var fileElement = xmlDoc.CreateElement("File");
                    var attrFileName = xmlDoc.CreateAttribute("FileName");
                    attrFileName.Value = currentDir + file.Name ;
                    fileElement.Attributes.Append(attrFileName);
                    var attrFileSize = xmlDoc.CreateAttribute("FileSize");
                    attrFileSize.Value = file.Length.ToString();
                    fileElement.Attributes.Append(attrFileSize);
                    updateFileList.AppendChild(fileElement);
                }
                //获取所有子目录
                var subDirs = root.GetDirectories();
                if(subDirs.Length>0)
                {
                    foreach (var subDir in subDirs)
                    {
                        getFile(subDir.FullName, updateFileList, currentDir + subDir + @"\");
                    }
                }
                else
                {
                    return;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
 

        
    }
}
