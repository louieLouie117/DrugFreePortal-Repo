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




const AddToCookies = (cookieName, item) =>{
    console.log("cookieName and item", cookieName, item);
    document.cookie = `${cookieName}= ${item}`;
}

const  getCookie = (cookieName) => {
        console.log("get cookies function");
        let cookieNeeded = cookieName + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');

        let cookieItemFound  = "test";
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(cookieNeeded) == 0) {
                console.log("cookie found in loop----", c.substring(cookieNeeded.length, c.length));
                cookieItemFound = c.substring(cookieNeeded.length, c.length)
                // return c.substring(cookieNeeded.length, c.length);

            }
        }

        console.log("****cookies found for use***", cookieItemFound)

        return cookieItemFound;
}