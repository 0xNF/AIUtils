using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace AIUtils {
    public class XMLio {

        public static async Task<T> ReadObjectFromXmlFileAsync<T>(string filename) {
            // this reads XML content from a file ("filename") and returns an object  from the XML
            T objectFromXml = default(T);
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(filename);
            Stream stream = await file.OpenStreamForReadAsync();
            objectFromXml = (T)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

        public static async Task<T> ReadObjectFromXMLStorageFileAsync<T>(StorageFile f) where T : class, new() {
            T objectFromXml = default(T);
            var serializer = new XmlSerializer(typeof(T));
            Stream stream = await f.OpenStreamForReadAsync();
            objectFromXml = (T)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

        public static async Task deleteXMLStorageFile(string filename) {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(filename + ".xml");
            await file.DeleteAsync();
        }

        public static async Task<IReadOnlyList<StorageFile>> getFolderItems() {
            StorageFolder listFolder = ApplicationData.Current.LocalFolder;
            var list = await listFolder.GetFilesAsync();
            list = list.Where(x => x.FileType == ".xml").ToList();
            List<StorageFile> lsf = new List<StorageFile>();
            foreach (StorageFile sf in list) {
                var fileProps = await sf.GetBasicPropertiesAsync();
                if (fileProps.Size > 0) {
                    lsf.Add(sf);
                }
            }
            return lsf;
        }

        public static async Task SaveObjectToXml<T>(T objectToSave, string filename) {
            // stores an object in XML format in file called 'filename'
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync()) {
                serializer.Serialize(stream, objectToSave);
                await stream.FlushAsync();
                stream.Dispose();
            }
        }

        public static async Task<bool> firstLaunch(string firstLaunchText) {
            StorageFolder listFolder = ApplicationData.Current.LocalFolder;
            var list = await listFolder.GetFilesAsync();
            list = list.Where(x => x.Name == firstLaunchText).ToList();
            if (list.Count == 0) {
                await SaveObjectToXml("firstlaunch", firstLaunchText);
                return true;
            }
            else {
                return false;
            }
        }

    }
}
