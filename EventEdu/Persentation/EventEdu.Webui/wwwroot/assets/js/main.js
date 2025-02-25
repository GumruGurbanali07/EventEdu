
(function ($) {
    "use strict";

    // Spinner (Hide after 1ms)
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();

    // Initiate WOW.js for animations
    new WOW().init();

    // Sticky Navbar
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.sticky-top').css('top', '0px');
        } else {
            $('.sticky-top').css('top', '-100px');
        }
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
        dots: false,
        loop: true,
        nav: true,
        navText: [
            '<i class="bi bi-chevron-left"></i>',
            '<i class="bi bi-chevron-right"></i>'
        ]
    });

    // Testimonials Carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        center: true,
        margin: 24,
        dots: true,
        loop: true,
        nav: false,
        responsive: {
            0: { items: 1 },
            768: { items: 2 },
            992: { items: 3 }
        }
    });

})(jQuery);

// General Slideshow
let index = 0;
const slides = document.querySelector('.slides');
const totalSlides = document.querySelectorAll('.slide').length;

function moveSlide(step) {
    index = (index + step + totalSlides) % totalSlides;
    slides.style.transform = `translateX(-${index * 100}%)`;
}

// Event Slider
document.querySelectorAll('.eventSlider-container').forEach(sliderContainer => {
    let currentIndex = 0;
    const totalEventSlides = sliderContainer.querySelectorAll('.eventSlide').length;
    const eventSlider = sliderContainer.querySelector('.eventSlider');
    const eventSlideWidth = 100 / 5;

    function moveEventSlide(direction) {
        currentIndex += direction;
        currentIndex = Math.max(0, Math.min(currentIndex, totalEventSlides - 5));
        eventSlider.style.transform = `translateX(-${currentIndex * eventSlideWidth}%)`;
    }

    setInterval(() => moveEventSlide(1), 3000);
    sliderContainer.querySelector('.prev').addEventListener('click', () => moveEventSlide(-1));
    sliderContainer.querySelector('.next').addEventListener('click', () => moveEventSlide(1));
});

//// Layout Active Classes For Per Page
document.addEventListener("DOMContentLoaded", function () {
    // Get current URL path
    let currentPath = window.location.pathname.toLowerCase();

    // Extract the controller name from the URL
    let pathSegments = currentPath.split("/");
    let controllerName = pathSegments.length > 1 ? pathSegments[1] : "";

    // Get all nav links
    let navLinks = document.querySelectorAll(".navbar-nav .nav-link");

    // Get the Events dropdown
    let eventDropdown = document.querySelector(".nav-item.dropdown .nav-link[data-bs-toggle='dropdown']");

    // Remove active class from all nav links initially
    navLinks.forEach(nav => nav.classList.remove("active"));

    if (controllerName === "account") {
        // If in Account controller, make sure no navbar link is active
        return;
    }

    let isEventPage = controllerName === "event";

    if (isEventPage && eventDropdown) {
        // If on an Event page, activate the Events dropdown
        eventDropdown.classList.add("active");
    } else {
        // Otherwise, activate the corresponding page link
        navLinks.forEach(link => {
            let linkPath = link.getAttribute("href").toLowerCase();
            if (currentPath === linkPath) {
                link.classList.add("active");
            }
        });
    }
});

// Auto-Slider for Sponsor's Page
function showSlider() {
    document.querySelectorAll('.slider').forEach(slider => slider.classList.remove('active'));
    let targetSlider = document.querySelector(window.location.hash);
    if (targetSlider) targetSlider.classList.add('active');
}
window.addEventListener('load', showSlider);
window.addEventListener('hashchange', showSlider);

// Gender Selection
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".gender-toggle button").forEach(button => {
        button.addEventListener("click", function () {
            document.querySelectorAll(".gender-toggle button").forEach(btn => btn.classList.remove("active"));
            this.classList.add("active");
        });
    });

    document.querySelector(".save").addEventListener("click", function (event) {
        event.preventDefault();
        alert("Your changes have been saved!");
    });

    document.querySelectorAll(".sidebar li").forEach(item => {
        item.addEventListener("click", function () {
            document.querySelectorAll(".sidebar li").forEach(i => i.classList.remove("active"));
            this.classList.add("active");
        });
    });
});

// Star Rating System
document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll(".send-review .stars .star");

    stars.forEach((star, index) => {
        star.addEventListener("click", function () {
            stars.forEach(s => s.classList.remove("active"));
            for (let i = 0; i <= index; i++) {
                stars[i].classList.add("active");
            }
        });
    });
});

// Participation Status Filter
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".filter").forEach(button => {
        button.addEventListener("click", function () {
            document.querySelectorAll(".filter").forEach(btn => btn.classList.remove("active"));
            this.classList.add("active");
        });
    });
});

// Pagination For Categories of Events Page
document.addEventListener("DOMContentLoaded", function () {
    let currentEPage = 1;
    const itemsPerEPage = 20; // Number of events per page
    const Eitems = document.querySelectorAll('.eventSlide'); // Make sure this class is correct
    const totalEPages = Math.ceil(Eitems.length / itemsPerEPage);

    const prevEBtn = document.getElementById('prevEventBtn');
    const nextEBtn = document.getElementById('nextEventBtn');

    function updateEventsPagination() {
        // Hide all event items initially
        Eitems.forEach(item => {
            item.style.display = 'none';
        });

        // Show items for the current page
        const startEIndex = (currentEPage - 1) * itemsPerEPage;
        const endEIndex = currentEPage * itemsPerEPage;

        for (let i = startEIndex; i < endEIndex && i < Eitems.length; i++) {
            Eitems[i].style.display = 'flex';
        }

        // Enable or disable buttons based on the current page
        prevEBtn.disabled = currentEPage === 1;
        nextEBtn.disabled = currentEPage === totalEPages;

        // Smooth scroll to the top of the page
        window.scrollTo({ top: 0, behavior: "smooth" });
    }


    prevEBtn.addEventListener('click', () => {
        if (currentEPage > 1) {
            currentEPage--;
            updateEventsPagination();
        }
    });

    nextEBtn.addEventListener('click', () => {
        if (currentEPage < totalEPages) {
            currentEPage++;
            updateEventsPagination();
        }
    });

    // Initial pagination setup
    updateEventsPagination();
});

// Pagination For Comments
let currentPage = 1;
const reviewsPerPage = 3;

function paginateReviews() {
    const reviews = document.querySelectorAll('.review');
    const totalReviews = reviews.length;

    // Hide all reviews
    reviews.forEach((review, index) => {
        review.style.display = 'none';
    });

    // Show only reviews for the current page
    for (let i = (currentPage - 1) * reviewsPerPage; i < currentPage * reviewsPerPage; i++) {
        if (reviews[i]) {
            reviews[i].style.display = 'flex';
        }
    }

    // Disable or enable the arrows based on current page
    const prevArrow = document.querySelector('.right-side .fa-arrow-left');
    const nextArrow = document.querySelector('.right-side .fa-arrow-right');
    const totalPages = Math.ceil(totalReviews / reviewsPerPage);

    // Disable the previous arrow if on the first page
    if (currentPage === 1) {
        prevArrow.disabled = true;
    } else {
        prevArrow.disabled = false;
    }

    // Disable the next arrow if on the last page
    if (currentPage === totalPages) {
        nextArrow.disabled = true;
    } else {
        nextArrow.disabled = false;
    }
}

// Event listener for the left (previous) arrow
document.querySelector('.right-side .fa-arrow-left').addEventListener('click', function () {
    if (currentPage > 1) {
        currentPage--;
        paginateReviews();
    }
});

// Event listener for the right (next) arrow
document.querySelector('.right-side .fa-arrow-right').addEventListener('click', function () {
    const reviews = document.querySelectorAll('.review');
    const totalPages = Math.ceil(reviews.length / reviewsPerPage);
    if (currentPage < totalPages) {
        currentPage++;
        paginateReviews();
    }
});

// Initialize pagination
paginateReviews();



// Pagination For Participation History Page
let currentHPage = 1;
const itemsPerHPage = 4; // Number of items per page
const Hitems = document.querySelectorAll('.participation-item');
const totalHPages = Math.ceil(Hitems.length / itemsPerHPage);

const prevHBtn = document.getElementById('prevEventHistoryBtn');
const nextHBtn = document.getElementById('nextEventHistoryBtn');

function updateHistoryPagination() {
    // Hide all items initially
    Hitems.forEach((item, index) => {
        item.style.display = 'none';
    });

    // Show items for the current page
    const startHIndex = (currentHPage - 1) * itemsPerHPage;
    const endHIndex = currentHPage * itemsPerHPage;

    for (let i = startHIndex; i < endHIndex && i < Hitems.length; i++) {
        Hitems[i].style.display = 'flex';
    }

    // Enable or disable buttons based on the current page
    prevHBtn.disabled = currentHPage === 1;
    nextHBtn.disabled = currentHPage === totalHPages;
}

prevHBtn.addEventListener('click', () => {
    if (currentHPage > 1) {
        currentHPage--;
        updateHistoryPagination(); // Corrected function name
    }
});

nextHBtn.addEventListener('click', () => {
    if (currentHPage < totalHPages) {
        currentHPage++;
        updateHistoryPagination(); // Corrected function name
    }
});

// Initial pagination setup
updateHistoryPagination();


//// Handle the click event for the heart (like) icon
//var heartIcon = document.getElementById('heart');
//heartIcon.addEventListener('click', function () {

//    // Toggle between regular and solid heart classes
//    if (heartIcon.classList.contains('fa-regular')) {
//        heartIcon.classList.remove('fa-regular');
//        heartIcon.classList.add('fa-solid');
//        heartIcon.style.color = "red";  // Set color to red when liked
//    } else {
//        heartIcon.classList.remove('fa-solid');
//        heartIcon.classList.add('fa-regular');
//        heartIcon.style.color = "";  // Remove color when unliked, restoring default
//    }
//});
