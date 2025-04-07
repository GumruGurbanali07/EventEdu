
const button = document.getElementById("post");
const sponsorId = document.getElementById("sponsorId").value();
const desc = document.getElementById("desc").value()
button.addEventListener("click", async () => {
    const url = "https://localhost:7057/feedback/create";

    const data = {
        name: "Test User",
        message: "Bu feedback test məqsədlidir"
    };

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            const result = await response.json();
            console.log("Serverdən cavab:", result);
        } else {
            console.error("Xəta baş verdi:", response.status);
        }
    } catch (error) {
        console.error("Şəbəkə xətası:", error);
    }
});