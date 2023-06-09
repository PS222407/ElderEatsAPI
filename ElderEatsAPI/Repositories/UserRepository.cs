﻿using ElderEatsAPI.Data;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderEatsAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public UserValidationDto Register(User user)
    {
        UserValidationDto userValidationDto = new UserValidationDto();
        if (FindUserByEmail(user.Email) != null)
        {
            userValidationDto.Reason = "Email is already taken, please go to login";
            return userValidationDto;
        }

        user.Token = Guid.NewGuid().ToString();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);

        if (!Save())
        {
            userValidationDto.Reason = "Something went wrong while registering user";
            return userValidationDto;
        }

        userValidationDto.User = user;
        return userValidationDto;
    }

    public UserValidationDto Login(User user)
    {
        UserValidationDto userValidationDto = new UserValidationDto();
        User? userDb = FindUserByEmail(user.Email);
        if (userDb == null)
        {
            userValidationDto.Reason = "User not found, please register first";
            return userValidationDto;
        }

        if (!BCrypt.Net.BCrypt.Verify(user.Password, userDb.Password))
        {
            userValidationDto.Reason = "Invalid credentials";
            return userValidationDto;
        }

        userValidationDto.User = userDb;
        return userValidationDto;
    }

    public User? GetUserByToken(string? token)
    {
        return token == null ? null : _context.Users.FirstOrDefault(u => u.Token == token);
    }

    private User? FindUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? FindUserById(long id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }

    public List<Account?>? getConnectedAccounts(int user, bool ActiveAccountsOnly = true)
    {
        List<AccountUser> accountusers = new List<AccountUser>();
        List<Account?> accounts = new List<Account?>();

        if (user != null)
        {
            if (ActiveAccountsOnly)
            {
                accountusers = _context.AccountUsers.Where(au => au.UserId == user && au.Status == 0).ToList();
            }
            else
            {
                accountusers = _context.AccountUsers.Where(au => au.UserId == user).ToList();
            }

            foreach (AccountUser accountUser in accountusers)
            {
                accounts.Add(_context.Accounts.Where(a => a.Id == accountUser.AccountId).FirstOrDefault());
            }
        }
        if(accounts.Count == 0)
        {
            accounts = null;
        }
        return accounts;
    }
}