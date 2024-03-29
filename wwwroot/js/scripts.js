console.log("scripts.js loaded!")

// arrow function for SignOutHandlder
const SignOutHandler = async () => {
    console.log("SignOutHandler called");
    fetch("/SignOutMethod")
        .then(response => response.json())
        .then(data => {
            console.log("Status", data);
        });
};