﻿namespace DMoreno.CashFlowControl.Domain.Interfaces.UoW;

public interface IUnitOfWork
{
    Task CommitAsync();
}
