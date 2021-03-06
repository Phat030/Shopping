﻿<script>
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();
    
    connection.start().catch(err => console.error(err.toString()));
    
connection.on('Send', (user, message) => {
        appendLine(nick, message);
});

document.getElementById('frm-send-message').addEventListener('submit', event => {
        let message = $('#message').val();
        let user = $('#spn-user').text();

    $('#message').val('');

    connection.invoke('Send', user, message);
    event.preventDefault();
});

function appendLine(user, message, color) {
        let nameElement = document.createElement('strong');
    nameElement.innerText = `${user}:`;

    let msgElement = document.createElement('em');
    msgElement.innerText = ` ${message}`;

    let li = document.createElement('li');
    li.appendChild(nameElement);
    li.appendChild(msgElement);

    $('#messages').append(li);
};

function continueToChat() {
        $('#spn-user').text($('#user').val());
    $('#entrance').hide();
    $('#chat').show();
    };

</script>