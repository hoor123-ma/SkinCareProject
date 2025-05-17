using SkinCareProject.Models;
using System.Linq;
using System.Web.Mvc;

public class HomeController : Controller
{
    private UsersEntities db = new UsersEntities();

    public ActionResult Index()
    {
        return View();
    }

    // صفحة التسجيل (GET)
    public ActionResult Register()
    {
        return View();
    }

    // صفحة التسجيل (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Register(User u)
    {
        if (ModelState.IsValid)
        {
            // تحقق هل البريد موجود مسبقاً لتجنب التكرار (اختياري)
            var existingUser = db.Users.FirstOrDefault(x => x.email == u.email);
            if (existingUser != null)
            {
                ModelState.AddModelError("email", "This email is already registered.");
                return View(u);
            }

            db.Users.Add(u);
            db.SaveChanges();

            ViewBag.Message = "Registration successful! You can now log in.";
            // نفرغ النموذج من القيم لنعرض صفحة نظيفة مع الرسالة
            ModelState.Clear();
            return View();
        }
        // لو البيانات غير صحيحة، ارجع نفس الصفحة مع الأخطاء والبيانات
        return View(u);
    }

    // صفحة تسجيل الدخول (GET)
    public ActionResult Login()
    {
        return View();
    }

    // تسجيل الدخول (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(User u)
    {
        if (ModelState.IsValid)
        {
            var user = db.Users.FirstOrDefault(x => x.email == u.email && x.password == u.password);
            if (user != null)
            {
                Session["UserID"] = user.ID;
                Session["UserEmail"] = user.email;

                ViewBag.Message = "Login successful. Welcome!";
                ModelState.Clear();
                return View();
            }
            else
            {
                // إضافة رسالة خطأ تظهر في الصفحة
                ModelState.AddModelError("", "Invalid email or password.");
                return View(u);
            }
        }
        // لو النموذج غير صالح، أرجع نفس الصفحة مع الأخطاء
        return View(u);
    }

    
}
