﻿using System;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.ViewModel;

namespace DevExpress.DevAV.Common.ViewModel {
    partial class SingleObjectViewModel<TEntity, TPrimaryKey, TUnitOfWork>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork {
        protected virtual bool EnableSelectedItemSynchronization {
            get { return false; }
        }
        protected override void OnInitializeInRuntime() {
            base.OnInitializeInRuntime();
            if(EnableSelectedItemSynchronization)
                SubscribeSelectedItemSynchronizationMessage();
        }
        protected void SubscribeSelectedItemSynchronizationMessage() {
            Messenger.Default.Register<SelectedItemSynchronizationMessage<TEntity>>(this, x => OnSelectedItemSynchronizationMessage(x));
        }
        protected virtual void OnSelectedItemSynchronizationMessage(SelectedItemSynchronizationMessage<TEntity> m) {
            if(m.Entity == null) Entity = null;
            else OnParameterChanged(Repository.GetPrimaryKey(m.Entity));
        }
        protected virtual bool EnableEntityChangedSynchronization {
            get { return false; }
        }
        protected override void OnEntityMessage(EntityMessage<TEntity, TPrimaryKey> msg) {
            if(EnableEntityChangedSynchronization && msg.MessageType == EntityMessageType.Changed) {
                if(object.Equals(msg.PrimaryKey, PrimaryKey))
                    Reload();
            }
            base.OnEntityMessage(msg);
        }
    }
}