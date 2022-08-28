using System;
using System.Text;
using UnityEngine;

namespace ENA.Utilities
{
    public class YAMLBuilder
    {
        #region Classes
        public struct Indentation: IDisposable
        {
            #region Variables
            YAMLBuilder builder;
            #endregion
            #region Constructors
            public Indentation(YAMLBuilder builder)
            {
                this.builder = builder;
                builder.IndentRight();
            }
            #endregion
            #region IDisposable Implementation
            public void Dispose()
            {
                builder.IndentLeft();
            }
            #endregion
        }
        #endregion
        #region Variables
        StringBuilder yaml;
        int currentIndentLevel = 0;
        #endregion
        #region Constructors
        public YAMLBuilder()
        {
            yaml = new StringBuilder();
        }
        #endregion
        #region Methods
        public void Block(string value, string comment = null)
        {
            InsertLineIndents();
            yaml.Append("- ").Append(value);
            if (!string.IsNullOrEmpty(comment)) {
                yaml.Append(" #").Append(comment);
            }
            yaml.AppendLine();
        }

        public void Header(string comment)
        {
            yaml.AppendLine(comment).AppendLine("---");
        }

        public Indentation Indent()
        {
            return new Indentation(this);
        }

        public void IndentRight()
        {
            currentIndentLevel++;
        }

        public void IndentLeft()
        {
            currentIndentLevel = Mathf.Max(currentIndentLevel-1,0);
        }

        private void InsertLineIndents()
        {
            for(int i = 0; i < currentIndentLevel; i++)
                yaml.Append("\t");
        }

        public void Mapping(string parameter, string value, string comment = null)
        {
            InsertLineIndents();
            yaml.Append(parameter).Append(": ").Append(value);
            if (!string.IsNullOrEmpty(comment)) {
                yaml.Append(" #").Append(comment);
            }
            yaml.AppendLine();
        }

        public string Output()
        {
            return yaml.ToString();
        }
        #endregion
    }
}