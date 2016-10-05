namespace Smart.Windows.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    public abstract class ViewModelBase : NotificationObject, IDataErrorInfo
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                var results = new List<ValidationResult>();
                if (Validator.TryValidateProperty(
                    GetType().GetProperty(columnName).GetValue(this, null),
                    new ValidationContext(this, null, null) { MemberName = columnName },
                    results))
                {
                    return null;
                }

                return results.First().ErrorMessage;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string Error
        {
            get
            {
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(
                    this,
                    new ValidationContext(this, null, null),
                    results))
                {
                    return string.Empty;
                }

                return String.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            }
        }

        /// <summary>
        ///
        /// </summary>
        private Messenger messenger;

        /// <summary>
        ///
        /// </summary>
        public Messenger Messenger
        {
            get { return messenger ?? (messenger = new Messenger()); }
        }

        /// <summary>
        ///
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messenger"></param>
        protected ViewModelBase(Messenger messenger)
        {
            this.messenger = messenger;
        }
    }
}
