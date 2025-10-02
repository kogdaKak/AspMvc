// ========== Бесконечный скролл ==========

// Текущая страница (начинаем с 1)
let currentPage = 1;

// Размер страницы подставляется из Razor (Index.cshtml)
let pageSize = window.pageSize || 6;

// Флаг, чтобы не было параллельных загрузок
let loading = false;

// Контейнер с постами и sentinel
const container = document.getElementById('postsContainer');
const sentinel = document.getElementById('scrollSentinel');

// Функция загрузки следующей страницы
async function loadNext() {
    if (loading) return;
    loading = true;
    currentPage++;

    try {
        const res = await fetch(`/Posts/List?page=${currentPage}`);
        if (!res.ok) {
            console.error('Ошибка загрузки:', res.status);
            loading = false;
            return;
        }

        const html = await res.text();

        // Если сервер вернул пусто — постов больше нет
        if (!html || html.trim().length === 0) {
            observer.disconnect();
            return;
        }

        // Вставляем новые посты внутрь .row
        container.querySelector('.row')
            .insertAdjacentHTML('beforeend', html);
    } catch (err) {
        console.error('Ошибка при fetch:', err);
    } finally {
        loading = false;
    }
}

// IntersectionObserver следит за sentinel
const observer = new IntersectionObserver((entries) => {
    if (entries[0].isIntersecting) {
        loadNext();
    }
}, { threshold: 0.1 });

// Запускаем наблюдение
observer.observe(sentinel);
