var sunMoon = document.getElementById("SunMoon");
sunMoon.onclick = function () {
    document.body.classList.toggle("darkMode");
    if (document.body.classList.contains("darkMode")) {
        sunMoon.src = "Logos/Sun.png";

    }
    else {
        sunMoon.src = "Logos/Moon.png";
    }
}
