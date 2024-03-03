const msgContainer = document.querySelector(".message-container")
const errorMsg= document.querySelector(".error-message")
const successMsg= document.querySelector(".success-message")

function initLoggedUserScreen() {
    const loggedUserMsg = document.querySelector("#logged-user-msg")
    if(loggedUserMsg != null && localStorage.getItem("username")) {
        var msg = loggedUserMsg.innerHTML
        msg = msg.replace("XYZ",localStorage.getItem("username"))
        loggedUserMsg.innerHTML = msg
    }
  }
  
initLoggedUserScreen();



document.getElementById("form")?.addEventListener("submit", function (event) {
    event.preventDefault();
});

const baseURL = 'https://localhost:7208'

async function createUser() {
    const url = baseURL+'/user/create-user';
    
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const passwordConfirmation = document.getElementById('confirmPassword').value;


    const userData = {
        username: username,
        email: email,
        password: password,
        confirmPassword: passwordConfirmation
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        if (responseData.success) {
            successMsg.innerHTML = responseData.message
            setTimeout(()=> {
                successMsg.innerHTML = ''
                location.reload()
                location.href = "Login.html"
            },3000)
            
        } else {
            msgContainer.style.display = 'block'
            errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                errorMsg.innerHTML = ''
            },4000)

        }

        console.log(responseData); 
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function Login() {
    const url = baseURL+'/user/login';
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const userData = {
        username: "",
        email: email,
        password: password,
        confirmPassword: ""
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        if (responseData.success) {
            successMsg.innerHTML = responseData.message
            localStorage.setItem('token', responseData.token);

            setTimeout(()=> {
                successMsg.innerHTML = ''
                location.href = "Logout.html"
            },3000)

   
        } else {
            msgContainer.style.display = 'block'
            errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                errorMsg.innerHTML = ''
            },4000)
        }

    } catch (error) {
        console.error('Fetch error:', error);
    }
}


async function Login() {
    const url = baseURL+'/user/login';
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const userData = {
        username: "",
        email: email,
        password: password,
        confirmPassword: ""
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        if (responseData.success) {
            successMsg.innerHTML = responseData.message
            localStorage.setItem('token', responseData.token);

            setTimeout(()=> {
                successMsg.innerHTML = ''
                location.href = "Logout.html"
            },3000)

   
        } else {
            msgContainer.style.display = 'block'
            errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                errorMsg.innerHTML = ''
            },4000)
        }

    } catch (error) {
        console.error('Fetch error:', error);
    }
}


async function Login() {
    const url = baseURL+'/user/login';
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    

    const userData = {
        username: "",
        email: email,
        password: password,
        confirmPassword: ""
    };

    try {
        const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
        const responseData = await response.json();
        if (responseData.success) {
            successMsg.innerHTML = responseData.message
            localStorage.setItem('token', responseData.token);
            localStorage.setItem('username', responseData.username);
            // loggedUserMsg.innerText.replace("XYZ", responseData.username)
            setTimeout(()=> {
                successMsg.innerHTML = ''
                location.href = "Logout.html"
            },3000)
   
        } else {
            msgContainer.style.display = 'block'
            errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                errorMsg.innerHTML = ''
            },4000)
        }

    } catch (error) {
        console.error('Fetch error:', error);
    }
}


async function testAuthentication() {
    const url = baseURL+'/user/test-authentication';

    try {
        const response = await fetch(url, {
        method: 'GET',
        headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`
        },
    });
        const responseData = await response.json();
        if (responseData.success) {
            successMsg.innerHTML = responseData.message
   
        } else {
            msgContainer.style.display = 'block'
            errorMsg.innerHTML = responseData.message
            setTimeout(()=> {
                errorMsg.innerHTML = ''
            },4000)
        }

    } catch (error) {
        console.error('Fetch error:', error);
    }
}