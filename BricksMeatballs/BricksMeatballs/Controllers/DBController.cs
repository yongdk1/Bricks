using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BricksMeatballs.Services;

/// <summary>
/// Author: Huang Chaoshan, Lim Pei Yan
/// The DBController aims to wait for the uraService finish running and returns the result after successfully run the batches and fetched the transaction records
/// </summary>
namespace BricksMeatballs.Controllers
{
    public class DBController : Controller { 
    private readonly UraService _uraService;

    public DBController(UraService uraService)
    {
        _uraService = uraService;
    }

    public async Task<IActionResult> Index()
    {
        await _uraService.GetPMI_Resi_Transaction(batch: 1);

        return View();
    }
}
}
