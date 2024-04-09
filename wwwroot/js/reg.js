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





const TermsHandler = (e) => {
    // Code for handling terms
    console.log('Terms handler was called');
    console.log(e.target.checked);
    let count = 0;

    if(e.target.checked === true){
        // check input field are empty
        const school = document.getElementById('StudentSchool').value;
        const studentId = document.getElementById('StudentId').value;
        const firstName = document.getElementById('StudentFirstName').value;
        const lastName = document.getElementById('StudentLastName').value;
        const phoneNumber  = document.getElementById('StudentPhoneNumber').value;
        const email = document.getElementById('StudentEmail').value;
        const password = document.getElementById('StudentPassword').value;

        // if statment to check if input are empty
        if(school === ""){
            document.getElementById('StudentSchoolLabel').style.color = "red";
            document.getElementById('StudentSchoolLabel').innerText = "can not be empty";
            count += 1;
        }
        if(studentId === ""){
            document.getElementById('StudentIdLabel').style.color = "red";
            document.getElementById('StudentIdLabel').innerText = "can not be empty";
            count += 1;

        }
        if(firstName === ""){
            document.getElementById('StudentFirstNameLabel').style.color = "red";
            document.getElementById('StudentFirstNameLabel').innerText = "can not be empty";
            count += 1;

        }
        if(lastName === ""){
            document.getElementById('StudentLastNameLabel').style.color = "red";
            document.getElementById('StudentLastNameLabel').innerText = "can not be empty";
            count += 1;

        }
        if(phoneNumber === ""){
            document.getElementById('StudentPhoneNumberLabel').style.color = "red";
            document.getElementById('StudentPhoneNumberLabel').innerText = "can not be empty";
            count += 1;

        }
        if(email === ""){
            document.getElementById('StudentEmailLabel').style.color = "red";
            document.getElementById('StudentEmailLabel').innerText = "can not be empty";
            count += 1;

        }
        if(password === ""){
            document.getElementById('StudentPasswordLabel').style.color = "red";
            document.getElementById('StudentPasswordLabel').innerText = "can not be empty";
            count += 1;

        }
    }else{
        document.getElementById('StudentRegBTN').disabled = true;
        console.log("not checked")
        return
    }
    console.log(count);
    if(count === 0){
        document.getElementById('StudentRegBTN').disabled = false;
    }else{
        e.target.checked = false;
    }
};