
using ThePlant.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType; // Эта директива 'using' кажется лишней и может быть удалена.

namespace UI.Controllers
{
    public class LoginAuthController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7181/api/Login"); // Убедитесь, что это правильный адрес вашего API
        private readonly HttpClient _httpClient;

        public LoginAuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        // GET-метод для отображения страницы входа
        public IActionResult LogIn()
        {
            return View();
        }

        // POST-метод для обработки данных входа
        [HttpPost]
     /*   public async Task<IActionResult> LogIn(User_Auth user_auth)
        {
            // Сериализация объекта DTO для отправки в API
            string givenData = JsonConvert.SerializeObject(user_auth);

            var url = baseAddress + "/authenticate"; // Полный URL для аутентификации
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(givenData, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode) // Если API вернул ошибку
                {
                    // Добавляем сообщение об ошибке в ModelState для отображения на View
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                }

                if (ModelState.IsValid) // Если все поля формы валидны и API вернул успешный ответ
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    // Сохраняем полученный токен
                    Services.TokenService.PostToken(data, HttpContext);
                    // Перенаправляем пользователя на главную страницу после успешного входа
                    return RedirectToAction("Index", "Home");
                }
            }

            // Если были ошибки, возвращаем View с моделью, чтобы пользователь мог исправить данные
            return View(user_auth);
        }*/

        // GET-метод для отображения страницы регистрации
        public IActionResult SignUp()
        {
            return View();
        }

        // POST-метод для обработки данных регистрации
        [HttpPost]
      /*  public async Task<IActionResult> SignUp(User_Registration user_Registration)
        {
            // Проверяем совпадение паролей
            if (user_Registration.Password != user_Registration.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Wrong confirm password");
            }

            // Сериализация объекта DTO для отправки в API
            string givenData = JsonConvert.SerializeObject(user_Registration);
            var url = baseAddress + "/register"; // Полный URL для регистрации
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(givenData, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode) // Если API вернул ошибку
                {
                    // Добавляем сообщение об ошибке в ModelState для отображения на View
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
                }

                if (ModelState.IsValid) // Если все поля формы валидны и API вернул успешный ответ
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    // Сохраняем полученный токен (если API возвращает токен после регистрации)
                    Services.TokenService.PostToken(data, HttpContext);

                    TempData["success"] = "User registered successfully"; // Сообщение об успешной регистрации
                    // Перенаправляем пользователя на страницу профилей или другую страницу
                    return RedirectToAction("Index", "Profiles"); // Убедитесь, что контроллер Profiles существует
                }
            }

            // Если были ошибки, возвращаем View с моделью, чтобы пользователь мог исправить данные
            return View(user_Registration);
        }
*/
        // Метод для выхода из системы (удаление токена)
        public IActionResult LogOut()
        {
            // Удаляем токен из куки
            Response.Cookies.Append("Token", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), // Устанавливаем срок действия в прошлое для удаления
                HttpOnly = true,
                Secure = true // Установите true, если используете HTTPS
            });

            // Перенаправляем пользователя на главную страницу
            return RedirectToAction("Index", "Home");
        }
    }
}