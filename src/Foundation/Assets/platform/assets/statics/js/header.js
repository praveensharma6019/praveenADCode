function displayListGroup() {
    var list = document.querySelector("#list-display");
    const selected1 = document.querySelector(".selected1");
    const optionList = document.querySelectorAll(".option");

    optionList.forEach((o) => {
        o.addEventListener("click", () => {
            selected1.src = o.querySelector("img").src;
            list.classList.add("hide");
        });
    });
}
