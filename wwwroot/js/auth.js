/*
 * Call the Authentication API
 */

export function SignIn(email, password, redirect) {

    var url = "/api/auth/signin";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    // Catch response
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        }
    };

    // Data to send
    var data = {
        email: email,
        password: password
    };

    // Call API
    xhr.send(JSON.stringify(data));
}

export function SignOut(redirect) {

    var url = "/api/auth/signout";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    // Catch response
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        }
    };

    // Call API
    xhr.send();
}
export async function Login(email,password,redirect) {
    try {
        const response = await fetch('/api/auth/signin', {
            method: 'POST',
            body: JSON.stringify({
                Email: email,
                Password: password,
            }),
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to sign in');
        }
        if (redirect)
            location.replace(redirect);
        const data = await response.json();

        const { success, message } = data;

        if (!success) {
            throw new Error(message);
        }

        console.log('Sign in successful');

    } catch (error) {
        console.error(error);
    }
}
