---
title: Backend-mediated authentication strategy for Blazor WASM
---

# Backend-mediated authentication (contoso.com/app → contoso.com)

Короткий опис схеми

- SPA (Blazor WASM) міститься за шляхом /app на тому ж домені contoso.com.
- Всі авторизаційні взаємодії відбуваються через backend (contoso.com).
- Browser отримує лише сесійну cookie від backend; токени (особливо refresh) зберігаються тільки на сервері.

## Потік (recommended)

1. Користувач заходить на захищену сторінку contoso.com/app/secure.
2. SPA перевіряє сесію через GET /auth/session (або виконує AJAX виклик до API). Якщо немає валідної сесії — NavigationManager.NavigateTo("/sabatex-login").
3. /sabatex-login (WASM) робить GET до backend: `GET /auth/challenge?returnUrl=/app/secure`.
4. Backend генерує state + nonce (запис в сесію/КЕШ/БД) і редиректить браузер на Microsoft Identity (Authorization Code, confidential client).
5. Microsoft після входу користувача редиректить на backend callback: `GET /auth/callback?code=...&state=...`.
6. Backend:
   - перевіряє state + nonce,
   - обмінює code на токени (сервер‑сервер) і валідовує id_token (aud, iss, exp, nonce),
   - створює внутрішню серверну сесію (DB/Redis/in‑memory) і встановлює cookie:
	 - Name: e.g. `.Contoso.Session`
	 - HttpOnly = true, Secure = true, SameSite = Lax (або Strict якщо підходить), Path = /
	 - короткий термін дії + slide/renew через backend при активності
   - редиректить браузер назад до returnUrl (наприклад /app/secure).
7. SPA робить запити до contoso.com/api/*; браузер автоматично додає сесійну cookie; backend верифікує сесію та обробляє запити.

## Критичні заходи безпеки

- Використовуйте Authorization Code (confidential client) на backend.
- Завжди перевіряйте state і nonce.
- Зберігайте refresh токени лише на сервері — ніколи в браузері.
- Cookie: HttpOnly + Secure + SameSite(Lax/Strict).
- CSRF: оскільки автентифікація базується на cookie, додайте CSRF-захист для state‑changing запитів (Double Submit Cookie або ASP.NET AntiForgery для API).
- HTTPS/TLS обов'язковий.
- Не логувати токени; логувати correlation id (state) для дебагу.
- Обмежити CORS лише до дозволених піддоменів (якщо застосовується). Якщо фронт і бек на одному домені — CORS не потрібен.

## Рекомендовані endpoint'и

- GET /auth/challenge?returnUrl=... — ініціює редирект до провайдера і зберігає state
- GET /auth/callback — OIDC callback
- GET /auth/session — перевірка стану сесії для SPA
- POST /auth/logout — закриття сесії і очищення cookie

## Практичні поради для Blazor WASM (Program.cs)

- HttpClient у WASM має надсилати запити з credentials (cookie). У Blazor WASM це означає, що при викликах fetch потрібно включити `credentials: 'include'`.
- У .NET можна встановити це через HttpClient у Hosted варіанті або через JS interop для fetch у standalone WASM. Перевірте Program.cs і налаштуйте HttpClient так, щоб cookie додавались автоматично при запитах до API на тому ж домені.
- На сторінці /sabatex-login показуйте повідомлення про помилку, якщо backend редиректнув з `?error=...`.

## Логіка сесій на backend

- Сесії можна зберігати в Redis або в БД; для простого демо — in‑memory (але для real‑world — розподілений кеш/DB).
- Сесія зберігає: user id, claims, access_token (короткостроково), refresh_token (в безпечному сховищі), expires_at.
- При закінченні access_token backend автоматично робить refresh (сервер‑сервер) і продовжує сесію.

## Обробка помилок UX

- При помилці авторизації редиректити на /sabatex-login?error=<code> і показувати дружнє повідомлення.
- Логувати server‑side деталі помилок з прив'язкою до state для розслідування.

## Подальші кроки (можливий список робіт)

1. Додати endpoints: /auth/challenge, /auth/callback, /auth/session, /auth/logout на backend.
2. Реалізувати session store (Redis/DB) і встановлення cookie.
3. Оновити `demo\SabatexBlazorDemo.WASMStandAloneWithIdentity\Program.cs` щоб:
   - робити запити до /auth/session при старті;
   - при відсутності сесії — редиректити на /sabatex-login;
   - використовувати HttpClient з credentials include (cookie).
4. Додати CSRF захист для POST запитів.

Якщо потрібно — реалізую конкретні зміни у вашому репозиторії (Program.cs та backend controllers). Напишіть «Реалізуй backend flow» і я почну з огляду Program.cs.
