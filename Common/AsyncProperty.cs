using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AsyncProperty<T> : INotifyPropertyChanged
                         where T : IConvertible, IComparable
    {
        private T property;
        private bool isInitialized = false;
        private bool isLoading = false;
        private Exception loadingException;
        private Task ignoredTask;
        private readonly IDataLoader dataLoader;

        public event PropertyChangedEventHandler PropertyChanged;

        public T Property
        {
            get
            {
                if (this.isInitialized)
                {
                    return this.property;
                }

                this.property = default(T);
                this.Init();
                return this.property;
            }

            set
            {
                if (value.CompareTo(this.property) == 0)
                {
                    return;
                }
                this.property = value;
                this.OnPropertyChanged();
            }
        }

        private void Init()
        {
            this.ignoredTask = this.InitializePropertyAsync();
        }

        public async Task InitializePropertyAsync()
        {
            this.IsLoading = true;
            try
            {
                Type type = typeof(T);
                if (type == typeof(int))
                {
                    this.Property = await this.dataLoader.GetIntValueAsync<T>(true);
                }
                else
                {
                    var result = await this.dataLoader.GetIntValueAsync<T>(false);
                    if (result != null)
                    {
                        this.IsLoading = false;
                        this.isInitialized = true;
                        this.Property = result;
                    }
                }

            }
            catch (Exception ex)
            {
                this.LoadingException = ex;
                this.Property = default(T);
                this.IsLoading = false;
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            private set
            {
                if (value == this.isLoading)
                {
                    return;
                }
                this.isLoading = value;
                OnPropertyChanged();
            }
        }

        public Exception LoadingException
        {
            get => this.loadingException;
            private set
            {
                if (value == this.loadingException)
                {
                    return;
                }

                this.loadingException = value;
                OnPropertyChanged();
            }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
