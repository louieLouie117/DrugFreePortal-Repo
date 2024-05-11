console.log("app.js loaded!")

// arrow function for SignOutHandlder
const SignOutHandler = async () => {
    console.log("SignOutHandler called");
    fetch("/SignOutMethod")
        .then(response => response.json())
        .then(data => {
            console.log("-------------data", data);
            console.log("-------------Status", data.status);
            if (data.status === "Sign Out Successfule") {
                console.log("SignOutHandler success");
                window.location.href = "/";
            } else {            
                console.log("SignOutHandler failed");
            }
           
        });
};


const SignInContainerHandler = async (e) => {
    // change the container to sign in to display grid id SignInContainer
    console.log("SignInContainerHandler called");
    document.getElementById("SignInContainer").style.display = "grid";
    e.target.style.display = "none";

}

