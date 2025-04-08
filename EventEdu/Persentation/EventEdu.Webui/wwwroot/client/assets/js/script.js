document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll(".star");
    const commentBox = document.getElementById("comment");
    const submitBtn = document.getElementById("submitBtn");
    const outputBox = document.getElementById("outputBox");

    const eventId = commentBox.dataset.id;
    let selectedRating = 0;

    // ⭐ Ulduz hover və click hadisələri
    stars.forEach((star, index) => {
        star.addEventListener("mouseover", () => {
            for (let i = 0; i <= index; i++) {
                stars[i].classList.add("hover");
            }
        });

        star.addEventListener("mouseout", () => {
            stars.forEach(s => s.classList.remove("hover"));
        });

        star.addEventListener("click", () => {
            selectedRating = index + 1;
            stars.forEach(s => s.classList.remove("selected"));
            for (let i = 0; i <= index; i++) {
                stars[i].classList.add("selected");
            }
        });
    });

    // ✅ Submit düyməsi ilə serverə göndərmək
    submitBtn.addEventListener("click", async () => {
        const comment = commentBox.value.trim();

        if (selectedRating === 0) {
            outputBox.textContent = "Zəhmət olmasa, ulduzla qiymətləndirin.";
            outputBox.style.color = "red";
            return;
        }

        if (!comment) {
            outputBox.textContent = "Zəhmət olmasa, bir rəy yazın.";
            outputBox.style.color = "red";
            return;
        }

        const data = {
            eventId: eventId,
            rating: selectedRating,
            comment: comment
        };
          console.log(data)
        try {
            const response = await fetch("https://localhost:7057/feedback/", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                const result = await response.json();
                outputBox.textContent = "Rəy uğurla göndərildi!";
                outputBox.style.color = "green";
                console.log("Serverdən cavab:", result);

                commentBox.value = "";
                stars.forEach(s => s.classList.remove("selected"));
                selectedRating = 0;

                await loadFeedbacks(); // Yeni rəyləri yenidən yüklə
            } else {
                outputBox.textContent = "Göndərmə zamanı xəta baş verdi.";
                outputBox.style.color = "red";
            }
        } catch (error) {
            outputBox.textContent = "Şəbəkə xətası baş verdi.";
            outputBox.style.color = "red";
            console.error("Şəbəkə xətası:", error);
        }
    });

    // 📥 Rəyləri yükləmək funksiyası
    async function loadFeedbacks() {
        const eventId = commentBox.dataset.id;
        const outputBox = document.getElementById("outputBox");

        const feedbackList = document.getElementById("feedbackList"); // Əgər yoxdursa, HTML-də yarat
        feedbackList.innerHTML = "";

        try {
            const response = await fetch(`https://localhost:7057/feedback/${eventId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {
                const feedbacks = await response.json();

                feedbacks.forEach(feedback => {
                    const feedbackItem = document.createElement("div");
                    feedbackItem.classList.add("feedback-item");

                    feedbackItem.innerHTML = `
                        <div style="background-color:#f8f9fa; padding:15px; border-radius:10px; box-shadow:0 2px 6px rgba(0,0,0,0.1); margin-bottom:15px;">
                            <div style="display:flex; align-items:center; justify-content:space-between;">
                                <h4 style="margin:0; color:#333;">${feedback.fullName}</h4>
                                <span style="font-size:1.2rem; color:#ffc107;">${"★".repeat(feedback.rating)}${"☆".repeat(5 - feedback.rating)}</span>
                                <span style="font-size:1.2rem; color:#ffc107;"> Total Rating${feedback.ratingTotal}</span>
                            </div>
                            <p style="margin-top:10px; color:#555;">${feedback.comment}</p>
                        </div>
                    `;

                    feedbackList.appendChild(feedbackItem);
                });
            } else {
                feedbackList.innerHTML = "Rəyləri yükləmək mümkün olmadı.";
            }
        } catch (error) {
            console.error("Şəbəkə xətası:", error);
            feedbackList.innerHTML = "Şəbəkə xətası baş verdi.";
        }
    }

    loadFeedbacks(); // səhifə yüklənən kimi rəyləri gətir
});
