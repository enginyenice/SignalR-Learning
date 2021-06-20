using Microsoft.AspNetCore.SignalR;
using SignalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{

    /*
   
        StronglyType => Bir dilin sizin objenizi bilmesi anlamına geliyor.
                        Bir liste oluştururken generic olarak string belirtiyoruz mesela
                            --> List<string>
                        farklı bir tip girersem beni uyar.
                        Avantajı kod yazarken hata yapma oranımızı azaltır.
                        
                        IProductHub->ReseiveProduct

     */
    public class ProductHub :Hub<IProductHub>
    {
        public async Task SendProduct(Product p)
        {
            await Clients.All.ReseiveProduct(p);
        }
    }
}
