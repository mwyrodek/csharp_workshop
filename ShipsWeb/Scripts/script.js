function sponsorTabControl() {
    $(".sponsors-tab--hook").click(() => {
        $(".sponsors-tab").toggleClass('opened');
    });

    $(".body-content, .navbar").click(() => {
        var adsExpanded = $(".sponsors-tab.opened").length > 0;

        if (adsExpanded) {
            $(".sponsors-tab").removeClass("opened");
        }
    });
}

function displaySwitchModal() {
    if (switchPlayers) {
        $(".switch-players").addClass('active');
    }

    $(".switch-players--button").click(() => {
        $(".switch-players").removeClass('active');
        $(".switch-players--button").off();
    });
}

$(document).ready(() => {
    $(".console-input").focus();

    sponsorTabControl();
    displaySwitchModal();
});