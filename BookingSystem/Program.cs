﻿using BookingSystem.Methods;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System.Collections.Generic;

namespace BookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<BookingSystemContext>()
               .UseSqlServer("Server=DESKTOP-SFTN8V0\\SQLEXPRESS;Database=BookingSystem;Trusted_Connection=True;TrustServerCertificate=true;")
               .Options;

            var dbContext = new BookingSystemContext(options);

            MenuManager.Run(dbContext);
        }

    }
}
