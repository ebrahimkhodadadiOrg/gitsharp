var services = this.HttpContext.RequestServices;
        var log = (ILog)services.GetService(typeof(ILog));