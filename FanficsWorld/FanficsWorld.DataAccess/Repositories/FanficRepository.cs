﻿using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorld.DataAccess.Repositories;

public class FanficRepository : IFanficRepository
{
    private readonly FanficsDbContext _context;

    public FanficRepository(FanficsDbContext context)
    {
        _context = context;
    }

    public async Task<Fanfic?> GetAsync(long id) =>
        await _context.Fanfics
            .Include(f => f.Author)
            .Include(f => f.Coauthors)
            .Include(f => f.FanficCoauthors)
            .SingleOrDefaultAsync(f => f.Id == id);

    public async Task<bool> AddAsync(Fanfic fanfic)
    {
        await _context.Fanfics.AddAsync(fanfic);
        return await _context.SaveChangesAsync() > 0;
    }
}