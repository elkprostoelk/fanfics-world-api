﻿using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.DataAccess.Interfaces;

public interface IFanficRepository
{
    Task<Fanfic?> GetAsync(long id);
    Task<bool> AddAsync(Fanfic fanfic);
    Task<bool> DeleteAsync(Fanfic fanfic);
    Task<bool> UpdateAsync(Fanfic fanfic);
    IQueryable<Fanfic> GetAllInProgressAsync(int skipCount, int takeCount);
    Task<long> CountAsync();
    Task UpdateRangeAsync(ICollection<Fanfic> changedFanfics);
}