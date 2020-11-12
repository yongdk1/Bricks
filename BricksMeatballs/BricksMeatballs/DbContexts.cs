using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BricksMeatballs
{
    public class AppDBContext : DbContext
    {
        public AppDBContext([NotNullAttribute] DbContextOptions<AppDBContext> options) : base(options)
        { }


    }
}