(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();


    // Initiate the wowjs
    new WOW().init();


    // Sticky Navbar
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.sticky-top').css('top', '0px');
        } else {
            $('.sticky-top').css('top', '-100px');
        }
    });


    // Dropdown on mouse hover
    const $dropdown = $(".dropdown");
    const $dropdownToggle = $(".dropdown-toggle");
    const $dropdownMenu = $(".dropdown-menu");
    const showClass = "show";

    $(window).on("load resize", function () {
        if (this.matchMedia("(min-width: 992px)").matches) {
            $dropdown.hover(
                function () {
                    const $this = $(this);
                    $this.addClass(showClass);
                    $this.find($dropdownToggle).attr("aria-expanded", "true");
                    $this.find($dropdownMenu).addClass(showClass);
                },
                function () {
                    const $this = $(this);
                    $this.removeClass(showClass);
                    $this.find($dropdownToggle).attr("aria-expanded", "false");
                    $this.find($dropdownMenu).removeClass(showClass);
                }
            );
        } else {
            $dropdown.off("mouseenter mouseleave");
        }
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Header carousel
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


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        center: true,
        margin: 24,
        dots: true,
        loop: true,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            }
        }
    });

})(jQuery);

let index = 0;
const slides = document.querySelector('.slides');
const totalSlides = document.querySelectorAll('.slide').length;

function moveSlide(step) {
    index += step;
    if (index < 0) {
        index = totalSlides - 1;
    } else if (index >= totalSlides) {
        index = 0;
    }
    slides.style.transform = `translateX(-${index * 100}%)`;
}
// Select all eventSliders on the page
const eventSliders = document.querySelectorAll('.eventSlider-container');

eventSliders.forEach(sliderContainer => {
    let currentIndex = 0;
    const totalEventSlides = sliderContainer.querySelectorAll('.eventSlide').length;
    const eventSlider = sliderContainer.querySelector('.eventSlider');
    const eventSlideWidth = 100 / 5; // Each eventSlide takes 20% of the width to show 5 slides

    // Function to move the eventSlider
    function moveEventSlide(direction) {
        currentIndex += direction;

        // Loop back to the first slide if we reach the end
        if (currentIndex < 0) {
            currentIndex = totalEventSlides - 5; // Go to the last 5 slides
        } else if (currentIndex > totalEventSlides - 5) {
            currentIndex = 0; // Go back to the first slides
        }

        // Update the eventSlider position
        eventSlider.style.transform = `translateX(-${currentIndex * eventSlideWidth}%)`;
    }

    // Auto slide every 5 seconds (adjusted for your request)
    setInterval(() => {
        moveEventSlide(1);
    }, 3000);

    // Event listeners for the previous and next buttons
    sliderContainer.querySelector('.prev').addEventListener('click', () => moveEventSlide(-1));
    sliderContainer.querySelector('.next').addEventListener('click', () => moveEventSlide(1));
});



function showSlider() {
    // Hide all sliders
    document.querySelectorAll('.slider').forEach(slider => {
        slider.classList.remove('active');
    });

    // Get the hash from URL (e.g., #slider1)
    let hash = window.location.hash;
    if (hash) {
        let targetSlider = document.querySelector(hash);
        if (targetSlider) {
            targetSlider.classList.add('active');
        }
    }
}

// Run when the page loads and when hash changes
window.addEventListener('load', showSlider);
window.addEventListener('hashchange', showSlider);



document.addEventListener("DOMContentLoaded", function () {
    // Gender Selection Toggle
    const genderButtons = document.querySelectorAll(".gender-toggle button");

    genderButtons.forEach(button => {
        button.addEventListener("click", function () {
            genderButtons.forEach(btn => btn.classList.remove("active"));
            this.classList.add("active");
        });
    });

    // Form Submission Alert
    document.querySelector(".save").addEventListener("click", function (event) {
        event.preventDefault();
        alert("Your changes have been saved!");
    });

    // Sidebar Navigation Highlight
    const sidebarItems = document.querySelectorAll(".sidebar li");

    sidebarItems.forEach(item => {
        item.addEventListener("click", function () {
            sidebarItems.forEach(i => i.classList.remove("active"));
            this.classList.add("active");
        });
    });
});

// Filter Participations Status
document.addEventListener("DOMContentLoaded", function () {
    const filterButtons = document.querySelectorAll(".filter");

    filterButtons.forEach(button => {
        button.addEventListener("click", function () {
            // Remove "active" class from all buttons
            filterButtons.forEach(btn => btn.classList.remove("active"));
            // Add "active" class to the clicked button
            this.classList.add("active");
        });
    });
});

let currentHPage = 1;
const itemsPerHPage = 4; // Number of items per page
const Hitems = document.querySelectorAll('.participation-item');
const totalHPages = Math.ceil(Hitems.length / itemsPerHPage);

const prevBtn = document.getElementById('prevBtn');
const nextBtn = document.getElementById('nextBtn');

function updateHistoryPagination() {
    // Hide all items initially
    Hitems.forEach((item, index) => {
        item.style.display = 'none';
    });

    // Show items for the current page
    const startIndex = (currentHPage - 1) * itemsPerHPage;
    const endIndex = currentHPage * itemsPerHPage;

    for (let i = startIndex; i < endIndex && i < Hitems.length; i++) {
        Hitems[i].style.display = 'flex';
    }

    // Enable or disable buttons based on the current page
    prevBtn.disabled = currentHPage === 1;
    nextBtn.disabled = currentHPage === totalHPages;
}

prevBtn.addEventListener('click', () => {
    if (currentHPage > 1) {
        currentHPage--;
        updateHistoryPagination(); // Corrected function name
    }
});

nextBtn.addEventListener('click', () => {
    if (currentHPage < totalHPages) {
        currentHPage++;
        updateHistoryPagination(); // Corrected function name
    }
});

// Initial pagination setup
updateHistoryPagination();






const itemsPerEPage = 10; // Number of items per page
const Eitems = document.querySelectorAll('.eventSlide');
const totalEPages = Math.ceil(Eitems.length / itemsPerEPage);
function updateEventsPagination() {
    // Hide all items initially
    Eitems.forEach((item, index) => {
        Eitem.style.display = 'none';
    });

    // Show items for the current page
    const startIndex = (currentPage - 1) * itemsPerEPage;
    const endIndex = currentPage * itemsPerEPage;

    for (let i = startIndex; i < endIndex && i < Eitems.length; i++) {
        items[i].style.display = 'flex';
    }

    // Enable or disable buttons based on the current page
    prevBtn.disabled = currentPage === 1;
    nextBtn.disabled = currentPage === totalEPages;
}

prevBtn.addEventListener('click', () => {
    if (currentPage > 1) {
        currentPage--;
        updatePagination();
    }
});

nextBtn.addEventListener('click', () => {
    if (currentPage < totalEPages) {
        currentPage++;
        updatePagination();
    }
});


// Initial pagination setup
updateEventsPagination();