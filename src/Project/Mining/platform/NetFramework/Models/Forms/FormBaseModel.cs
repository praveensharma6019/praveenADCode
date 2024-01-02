using System;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.Forms
{
    public class FormBaseModel
    {
        public Guid Id { get; set; }

        public Guid FormDefinationId { get; set; }

        public bool IsRedacted { get; set; }

        public DateTime Created { get; set; }

        public List<FormFieldData> objlistformFieldDatas = new List<FormFieldData>();

        public FormFileStorage formFileStorage = new FormFileStorage();
    }

    public class FormFieldData
    {
        public Guid FormEntryId { get; set; }
        public Guid FormDefinationId { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    public class FormFileStorage
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public bool Committed { get; set; }
        public DateTime Created { get; set; }
    }
}