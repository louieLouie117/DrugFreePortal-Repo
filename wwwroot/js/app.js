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



