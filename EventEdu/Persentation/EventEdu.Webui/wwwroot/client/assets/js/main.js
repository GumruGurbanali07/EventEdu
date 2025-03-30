(function ($) {
    "use strict";

    // Spinner (Hide after 1ms)
    setTimeout(() => $('#spinner').removeClass('show'), 1);

    // Initiate WOW.js for animations
    new WOW().init();

    // Sticky Navbar
    $(window).scroll(function () {
        $('.sticky-top').css('top', $(this).scrollTop() > 300 ? '0px' : '-100px');
    });

    // Dropdown on Hover (Desktop only)
    $(window).on("load resize", function () {
        if (window.matchMedia("(min-width: 992px)").matches) {
            $(".dropdown").hover(
                function () {
                    $(this).addClass("show").find(".dropdown-toggle").attr("aria-expanded", "true");
                    $(this).find(".dropdown-menu").addClass("show");
                },
                function () {
                    $(this).removeClass("show").find(".dropdown-toggle").attr("aria-expanded", "false");
                    $(this).find(".dropdown-menu").removeClass("show");
                }
            );
        } else {
            $(".dropdown").off("mouseenter mouseleave");
        }
    });

    // Back to Top Button
    $(window).scroll(function () {
        $('.back-to-top').toggle($(this).scrollTop() > 300);
    });

    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });

    // Header Carousel
    $(".header-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        items: 1,
        loop: true,
        nav: true,
        navText: ['<i class="bi bi-chevron-left"></i>', '<i class="bi bi-chevron-right"></i>']
    });

    // Testimonials Carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        center: true,
        margin: 24,
        dots: true,
        loop: true,
        responsive: { 0: { items: 1 }, 768: { items: 2 }, 992: { items: 3 } }
    });
})(jQuery);

// General Slideshow
const slides = document.querySelector('.slides');
if (slides) {
    let index = 0;
    const totalSlides = document.querySelectorAll('.slide').length;
    function moveSlide(step) {
        index = (index + step + totalSlides) % totalSlides;
        slides.style.transform = `translateX(-${index * 100}%)`;
    }
}

// Event Slider
const eventSliders = document.querySelectorAll('.eventSlider-container');
eventSliders.forEach(sliderContainer => {
    let currentIndex = 0;
    const totalSlides = sliderContainer.querySelectorAll('.eventSlide').length;
    const eventSlider = sliderContainer.querySelector('.eventSlider');

    function moveSlide(direction) {
        currentIndex = Math.max(0, Math.min(currentIndex + direction, totalSlides - 5));
        eventSlider.style.transform = `translateX(-${currentIndex * 20}%)`;
    }

    setInterval(() => moveSlide(1), 3000);
    sliderContainer.querySelector('.prev')?.addEventListener('click', () => moveSlide(-1));
    sliderContainer.querySelector('.next')?.addEventListener('click', () => moveSlide(1));
});

// Navbar Active State
document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname.toLowerCase().split("/")[1] || "";
    document.querySelectorAll(".navbar-nav .nav-link").forEach(link => {
        link.classList.toggle("active", link.getAttribute("href").toLowerCase() === `/${currentPath}`);
    });
});

// Gender Selection
const genderButtons = document.querySelectorAll(".gender-toggle button");
genderButtons.forEach(button => {
    button.addEventListener("click", function () {
        genderButtons.forEach(btn => btn.classList.remove("active"));
        this.classList.add("active");
    });
});

// Star Rating System
const stars = document.querySelectorAll(".send-review .stars .star");
stars.forEach((star, index) => {
    star.addEventListener("click", function () {
        stars.forEach((s, i) => s.classList.toggle("active", i <= index));
    });
});

// Participation Status Filter
const filters = document.querySelectorAll(".filter");
filters.forEach(button => {
    button.addEventListener("click", function () {
        filters.forEach(btn => btn.classList.remove("active"));
        this.classList.add("active");
    });
});

// Pagination Functions
function paginateItems(containerSelector, itemSelector, itemsPerPage, prevBtnSelector, nextBtnSelector) {
    let currentPage = 1;
    const items = document.querySelectorAll(itemSelector);
    const totalPages = Math.ceil(items.length / itemsPerPage);

    function updatePagination() {
        items.forEach((item, i) => item.style.display = (i >= (currentPage - 1) * itemsPerPage && i < currentPage * itemsPerPage) ? 'flex' : 'none');
        document.querySelector(prevBtnSelector)?.classList.toggle("disabled", currentPage === 1);
        document.querySelector(nextBtnSelector)?.classList.toggle("disabled", currentPage === totalPages);
    }

    document.querySelector(prevBtnSelector)?.addEventListener('click', () => { if (currentPage > 1) { currentPage--; updatePagination(); } });
    document.querySelector(nextBtnSelector)?.addEventListener('click', () => { if (currentPage < totalPages) { currentPage++; updatePagination(); } });
    updatePagination();
}

paginateItems('.eventSlide-container', '.eventSlide-item', 20, '.prevEventBtn', '.nextEventBtn');
paginateItems('.reviews-container', '.review', 3, '.fa-arrow-left', '.fa-arrow-right');
paginateItems('.participation-history', '.participation-item', 4, '.prevEventHistoryBtn', '.nextEventHistoryBtn');

// Like Button Toggle
const heartIcon = document.getElementById('heart');
heartIcon?.addEventListener('click', function () {
    this.classList.toggle('fa-regular');
    this.classList.toggle('fa-solid');
    this.style.color = this.classList.contains('fa-solid') ? "red" : "";
});

// Share Modal
const shareBtn = document.getElementById('shareBtn');
const shareModal = document.getElementById('shareModal');
shareBtn?.addEventListener('click', () => { shareModal.style.display = 'block'; document.getElementById('pageUrl').value = window.location.href; });
document.querySelector('.close')?.addEventListener('click', () => { shareModal.style.display = 'none'; });
window.addEventListener('click', e => { if (e.target === shareModal) shareModal.style.display = 'none'; });

// Debugging Search Input
const searchInput = document.getElementById("searchInput");
const searchBody = document.getElementById("searchBody"); // Fix the typo in the ID

searchInput?.addEventListener("keyup", function () {
    console.log(this.value);

    fetch(`/sponsor/search?search=${encodeURIComponent(this.value)}`)
        .then(res => res.text())
        .then(data => {
            searchBody.innerHTML = data; // Corrected from `.html(data)`
        })
        .catch(error => console.error('Error fetching data:', error));
});
