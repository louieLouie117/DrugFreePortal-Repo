console.log('reg.js is loaded');


const getWorkingURL = () => {
    const runningUrl = window.location.href;
    console.log('runningUrl', runningUrl);
    let localDevelopmentUrl = 'http://localhost:5043';
    let developemtServerUrl = 'http://13.58.200.202';
    let cleintSeverUrl = 'http://3.135.214.156';

    let workingURL = "";

    if (runningUrl === localDevelopmentUrl +"/") {
        workingURL = localDevelopmentUrl ;
    } else if (runningUrl === developemtServerUrl +"/") {
        workingURL = developemtServerUrl ;
    } else if (runningUrl === cleintSeverUrl +"/") {
        workingURL = cleintSeverUrl;
    }

    return workingURL;
}


const signInHandlerFetch = async (e) => {
    e.preventDefault();

    const workingURL = getWorkingURL();
    // Rest of the code...
    console.log('workingURL SignInhandlerFetch', workingURL);
    // Get the email and password from input fields
    const email = document.getElementById('signInemail').value;
    const password = document.getElementById('signInPassword').value;

    // Construct the complete URL

    // Create an object with the email and password
    const jsonData = {
        email: email,
        password: password
    };
    console.log('jsonData', jsonData);



        fetch(`${workingURL}/LoginFetch`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonData)
        })
            .then(response => response.json())
            .then(data => {
                // Handle the response data
                console.log("results from",data);
                if (data.fields.length > 0) {
                    console.log("data.fields", data.fields)
                    data.fields.forEach((field) => {
                        if (field === "Email") {
                            document.getElementById("signInEmailLabel").style.color = "red";
                            document.getElementById("signInEmailLabel").innerText = "can not be empty";
                        }
                        if (field === "Password") {
                            document.getElementById("signInPasswordLabel").style.color = "red";
                            document.getElementById("signInPasswordLabel").innerText = "can not be empty";
                        }
                    })
                }
        
                if (data.status === "Login Fetch Successfule") {
                    console.log("Login Fetch Successfule");
                    console.log("naviate to:", workingURL + "/dashboard");
                    alert("naviate to dashboard");
                    window.location.href = `${workingURL}/dashboard`;
                }
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });

};




