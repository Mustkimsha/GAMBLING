window.addEventListener('load', function () {
    var status = document.getElementById("status");
    var log = document.getElementById("log");

    function updateOnlineStatus(event) {
        var condition = navigator.onLine ? "online" : "offline";

        status.className = condition;
        status.innerHTML = condition.toUpperCase();

        if (condition == 'offline') {
            $('.internet').show();
            $('.show').hide();
            $('.agent-wrapper').hide();
            $('.modal-backdrop').hide();
        } else {
            $('.show').show();
            $('.internet').hide();
            $('.agent-wrapper').show();
            $('.modal-backdrop').show();
            // $('body').css('overflow', '');
        }
        // log.insertAdjacentHTML("beforeend", "Event: " + event.type + "; Status: " + condition);
    }

    window.addEventListener('online', updateOnlineStatus);
    window.addEventListener('offline', updateOnlineStatus);
});