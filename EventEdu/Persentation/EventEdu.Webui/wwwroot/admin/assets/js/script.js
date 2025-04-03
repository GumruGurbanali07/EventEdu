const { event } = require("jquery");

const addPartialButton = document.getElementById("addPartialButton");
const socialFormsContainer = document.getElementById("socialFormsContainer");
const form = document.getElementById("form");
let file: File | null;
let name = document.getElementById("name").value();
let url = document.getElementById("url").value()

const addSocial = document.getElementById("addSosial");



uploadFile(event){

    var input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
        connst image = input.files[0];
        this.file = image;
    }
}

addSocial.addEventListener("click", () => {
    const formData = new FormData()
    fetch("/Speaker/Create", {
        method: "POST",
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            console.log("Form submitted successfully:", data);

            // Handle success (e.g., show a success message, reset the form)
        })
        .catch(error => {
            console.error("Error submitting the form:", error);
            // Handle error
        });
})




addPartialButton.addEventListener("click", function () {




    const formClone = form.cloneNode(true);
    socialFormsContainer.appendChild(formClone);
});
