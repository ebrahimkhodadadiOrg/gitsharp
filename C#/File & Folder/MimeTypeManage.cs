using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BuildingBlocks.FileUtility
{
    public enum UploadType
    {
        [Description("همه")]
        All = 0,
        [Description("عکس")]
        Image = 1,
        [Description("سند")]
        Doc = 2
    }

    public static class MimeTypeManage
    {
        public static AccMimeTypeCollection AccMimeTypeCollection { get; set; }
        static MimeTypeManage()
        {
            AccMimeTypeCollection = new AccMimeTypeCollection();
            AccMimeTypeCollection.ConstractMimeTypes();
        }
    }
    public class AccMimeTypeCollection
    {
        List<AccMimeType> _accMimeTypes = new List<AccMimeType>();
        public List<AccMimeType> AccMimeTypes
        {
            get
            {
                return _accMimeTypes;
            }
        }
        public AccMimeTypeCollection()
        {

        }
        public void ConstractMimeTypes()
        {
            int index = 1;
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".jpg", MimeType = "image/jpeg", UploadType = UploadType.Image });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".png", MimeType = "image/png", UploadType = UploadType.Image });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".gif", MimeType = "image/gif", UploadType = UploadType.Image });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".webp", MimeType = "image/webp", UploadType = UploadType.Image });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".csv", MimeType = "text/csv", UploadType = UploadType.Doc });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".doc", MimeType = "application/msword", UploadType = UploadType.Doc });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".docx", MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", UploadType = UploadType.Doc });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".txt", MimeType = "text/plain", UploadType = UploadType.Doc });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".xls", MimeType = "application/vnd.ms-excel", UploadType = UploadType.Doc });
            _accMimeTypes.Add(new AccMimeType { ID = index++, Extention = ".xlsx", MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", UploadType = UploadType.Doc });
        }
        public List<AccMimeType> this[UploadType type]
        {
            get
            {
                if (type == UploadType.All)
                    return _accMimeTypes;
                return _accMimeTypes.Where(p => p.UploadType == type).ToList();
            }
        }
        public List<AccMimeType> this[string mimeOrExtention]
        {
            get
            {
                return _accMimeTypes.Where(p => p.MimeType == mimeOrExtention || p.Extention == mimeOrExtention).ToList();
            }
        }
    }
    public class AccMimeType
    {
        public int ID { get; set; }
        public string MimeType { get; set; }
        public string Extention { get; set; }
        public UploadType UploadType { get; set; }
    }
    public class UploadFileType
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
