console.log('reg.js is loaded');

window.onload = function () {
    getSchoolsForStudentReg();
};


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
                if (data.status === "password error") {
                    document.getElementById("signInEmailLabel").style.color = "red";
                    document.getElementById("signInEmailLabel").innerText = "Some of your info isn't correct please try again..";
                    return
                }

                if (data.status === "email error no user found") {    
                    document.getElementById("signInEmailLabel").style.color = "red";
                    document.getElementById("signInEmailLabel").innerText = "Some of your info isn't correct please try again.";
                    return
                }

                if(data.fields.length === 2) {
                    document.getElementById("signInEmailLabel").style.color = "red";
                    document.getElementById("signInEmailLabel").innerText = "Please enter your email and password.";
                    return
                }

                if (data.fields.length > 0) {
                    console.log("data.fields", data.fields)
                    data.fields.forEach((field) => {
                        console.log("field", field);
                        document.getElementById("signInEmailLabel").innerText = " ";
                       if (field === "Email") {
                            document.getElementById("signInEmailLabel").style.color = "red";
                            document.getElementById("signInEmailLabel").innerText = "Please enter your email.";
                        } else if (field === "Password") {
                            document.getElementById("signInEmailLabel").style.color = "red";
                            document.getElementById("signInEmailLabel").innerText = "Please enter your password.";
                        }
                    })
                }
                // login in bug fixed 6/6/2024
                if (data.status === "Login Fetch Successfule") {
                    console.log("Login Fetch Successful");
                    console.log("naviate to:", workingURL + "/dashboard");
                    window.location.href = `${workingURL}/dashboard`;
                }

                
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });

};





const getSchoolsForStudentReg = () => {
    fetch('/GetAllSchools', {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            // Handle success response
            console.log("schools--------", data.schoolData);
            RenderSchoolsOptionsForStudentsReg(data.schoolData);

        })
        .catch(error => {
            // Handle error response
            console.log(error);
        });
};


function RenderSchoolsOptionsForStudentsReg(schools) {
    const StudentSchoolSelector = document.getElementById('StudentSchoolSelector');
    StudentSchoolSelector.innerHTML = '<option>Select School</option>';

    schools.forEach(school => {
        console.log("school option", school.schoolId);

        const option = document.createElement('option');
        option.value = school.name; // It's common to use the id as the value
        option.textContent = school.name;
        option.id = school.schoolId;
        StudentSchoolSelector.appendChild(option);
    });

    // Add the event listener to the select element
    StudentSchoolSelector.addEventListener('change', addIdToInputStudentReg);
}



function addIdToInputStudentReg(event) {
    // The value of the selected option is the schoolId
    
    const selectedSchoolValue = event.target.value;
    if (selectedSchoolValue === "Select School") {
        document.getElementById('StudentSchool').value = "";
        return;
    }
    console.log("for student reg-----------",selectedSchoolValue);
   
    document.getElementById('StudentSchool').value = selectedSchoolValue;
    document.getElementById('SchoolIdForStudentReg').value = event.target.selectedOptions[0].id;
}

const ShowPassword = (e) => {
    e.preventDefault();
    const password = document.getElementById('signInPassword');
    const showButton = document.getElementById('showPasswordButton');
    if (password.type === 'password') {
        password.type = 'text';
        showButton.value = 'Hide';
    } else {
        password.type = 'password';
        showButton.value = 'Show';
    }
}