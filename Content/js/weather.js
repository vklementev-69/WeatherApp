/**
 * Weather App JavaScript - jQuery version
 * Handles data refresh and UI updates
 */

$(document).ready(function() {
    // Bind refresh button click
    $('#refreshBtn').on('click', refreshWeather);
    
    // Bind retry button click
    $('#retryBtn').on('click', refreshWeather);
    
    // Start auto-refresh timer
    startAutoRefresh();
    
    // Add keyboard shortcut (Ctrl+R)
    $(document).on('keydown', function(e) {
        if (e.key === 'r' && e.ctrlKey) {
            e.preventDefault();
            refreshWeather();
        }
    });
});

// Configuration
var CONFIG = {
    refreshInterval: 10 * 60 * 1000 // 10 minutes
};

var isRefreshing = false;
var refreshTimer = null;

/**
 * Refresh weather data
 */
function refreshWeather() {
    if (isRefreshing) return;
    
    isRefreshing = true;
    
    // Show loading state
    showLoading();
    
    // Add spinning animation to refresh button
    var $refreshBtn = $('#refreshBtn');
    var $icon = $refreshBtn.find('.glyphicon');
    $icon.addClass('glyphicon-spin');
    $refreshBtn.prop('disabled', true);
    
    // Make AJAX request
    $.ajax({
        url: '/Weather/RefreshWeather',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function(data) {
            if (data.HasError) {
                showError(data.ErrorMessage);
            } else {
                updateUI(data);
                showContent();
            }
        },
        error: function(xhr, status, error) {
            showError('Ошибка при обновлении данных. Пожалуйста, попробуйте позже.');
            console.error('Error:', error);
        },
        complete: function() {
            isRefreshing = false;
            $icon.removeClass('glyphicon-spin');
            $refreshBtn.prop('disabled', false);
            
            // Reset auto-refresh timer
            startAutoRefresh();
        }
    });
}

/**
 * Show loading state
 */
function showLoading() {
    $('#loadingState').show();
    $('#errorState').hide();
    $('#weatherContent').hide();
}

/**
 * Show error state
 */
function showError(message) {
    $('#loadingState').hide();
    $('#errorState').show();
    $('#weatherContent').hide();
    $('#errorMessage').text(message || 'Произошла ошибка');
}

/**
 * Show weather content
 */
function showContent() {
    $('#loadingState').hide();
    $('#errorState').hide();
    $('#weatherContent').show().hide().fadeIn(500);
}

/**
 * Update UI with new data
 */
function updateUI(data) {
    // Update current weather
    updateCurrentWeather(data.Current);
    
    // Update hourly forecast
    updateHourlyForecast(data.Hourly);
    
    // Update daily forecast
    updateDailyForecast(data.Daily);
    
    // Update last update time
    updateLastUpdateTime(data.Current ? data.Current.LastUpdated : null);
}

/**
 * Update current weather section
 */
function updateCurrentWeather(current) {
    if (!current) return;
    
    // Update icon
    $('.weather-icon-large img').attr('src', current.ConditionIcon).attr('alt', current.Condition);
    
    // Update temperature
    $('.temp-value').text(Math.round(current.Temperature));
    
    // Update condition
    $('.condition-text').text(current.Condition);
    
    // Update details table
    var $detailsTable = $('.table-details tbody');
    $detailsTable.find('tr:eq(0) td:eq(1) strong').text(Math.round(current.FeelsLike) + '°C');
    $detailsTable.find('tr:eq(1) td:eq(1) strong').text(current.Humidity + '%');
    $detailsTable.find('tr:eq(2) td:eq(1) strong').text(current.WindSpeed + ' км/ч ' + current.WindDirection);
    $detailsTable.find('tr:eq(3) td:eq(1) strong').text(current.Pressure + ' гПа');
    $detailsTable.find('tr:eq(4) td:eq(1) strong').text(current.Visibility + ' км');
}

/**
 * Update hourly forecast section
 */
function updateHourlyForecast(hourly) {
    if (!hourly || !Array.isArray(hourly)) return;
    
    var $container = $('#hourlyContainer');
    $container.empty();
    
    $.each(hourly, function (index, hour) {
        var timestamp = parseInt(hour.Time.match(/\d+/)[0]);
        var time = new Date(timestamp);
        var timeStr = ('0' + time.getHours()).slice(-2) + ':' + ('0' + time.getMinutes()).slice(-2);
        var rainHtml = hour.ChanceOfRain > 0 
            ? '<span class="label label-info">' + hour.ChanceOfRain + '%</span>' 
            : '<span>-</span>';
        
        var row = '<tr>' +
            '<td>' + timeStr + '</td>' +
            '<td class="text-center"><img src="' + hour.ConditionIcon + '" alt="' + hour.Condition + '" style="width: 40px; height: 40px;" /></td>' +
            '<td class="text-center"><strong>' + Math.round(hour.Temperature) + '°C</strong></td>' +
            '<td class="text-center">' + rainHtml + '</td>' +
        '</tr>';
        
        $container.append(row);
    });
}

/**
 * Update daily forecast section
 */
function updateDailyForecast(daily) {
    if (!daily || !Array.isArray(daily)) return;
    
    var $container = $('#dailyContainer');
    $container.empty();
    
    var monthNames = [
        'января', 'февраля', 'марта', 'апреля', 'мая', 'июня',
        'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря'
    ];
    
    $.each(daily, function(index, day) {
        const datestamp = parseInt(day.Date.match(/\d+/)[0]);
        var date = new Date(datestamp);
        var dateStr = date.getDate() + ' ' + monthNames[date.getMonth()];
        
        var html = '<div class="col-md-4 col-sm-12 daily-item">' +
            '<div class="thumbnail">' +
                '<div class="caption">' +
                    '<h3>' + day.DayOfWeek + ' <small>' + dateStr + '</small></h3>' +
                    '<div class="text-center">' +
                        '<img src="' + day.ConditionIcon + '" alt="' + day.Condition + '" style="width: 64px; height: 64px;" />' +
                        '<p class="lead">' + day.Condition + '</p>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-xs-6 text-center">' +
                            '<span class="label label-danger" style="font-size: 16px;">' + Math.round(day.MaxTemp) + '°</span>' +
                            '<p class="text-muted">Макс</p>' +
                        '</div>' +
                        '<div class="col-xs-6 text-center">' +
                            '<span class="label label-primary" style="font-size: 16px;">' + Math.round(day.MinTemp) + '°</span>' +
                            '<p class="text-muted">Мин</p>' +
                        '</div>' +
                    '</div>' +
                    '<hr />' +
                    '<div class="row">' +
                        '<div class="col-xs-6">' +
                            '<p><span class="glyphicon glyphicon-tint"></span> ' + day.ChanceOfRain + '%</p>' +
                        '</div>' +
                        '<div class="col-xs-6">' +
                            '<p><span class="glyphicon glyphicon-flag"></span> ' + day.MaxWindSpeed + ' км/ч</p>' +
                        '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-xs-6">' +
                            '<p><span class="glyphicon glyphicon-arrow-up"></span> ' + day.Sunrise + '</p>' +
                        '</div>' +
                        '<div class="col-xs-6">' +
                            '<p><span class="glyphicon glyphicon-arrow-down"></span> ' + day.Sunset + '</p>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</div>';
        
        $container.append(html);
    });
}

/**
 * Update last update time
 */
function updateLastUpdateTime(lastUpdated) {
    var $footer = $('footer');
    $footer.find('p:last-child').remove();
    
    if (lastUpdated) {
        $footer.append('<p>Последнее обновление: ' + lastUpdated + '</p>');
    }
}

/**
 * Start auto-refresh timer
 */
function startAutoRefresh() {
    // Clear existing timer
    if (refreshTimer) {
        clearInterval(refreshTimer);
    }
    
    // Set new timer
    refreshTimer = setInterval(function() {
        // Only auto-refresh if page is visible
        if (!document.hidden) {
            refreshWeather();
        }
    }, CONFIG.refreshInterval);
}

/**
 * Stop auto-refresh timer
 */
function stopAutoRefresh() {
    if (refreshTimer) {
        clearInterval(refreshTimer);
        refreshTimer = null;
    }
}

// Handle page visibility changes
document.addEventListener('visibilitychange', function() {
    if (document.hidden) {
        stopAutoRefresh();
    } else {
        startAutoRefresh();
    }
});

// Handle online/offline events
window.addEventListener('online', function() {
    console.log('Connection restored');
});

window.addEventListener('offline', function() {
    console.log('Connection lost');
    showError('Отсутствует подключение к интернету. Проверьте соединение.');
});

// Add custom CSS for spinning animation
$(function() {
    $('<style>')
        .prop('type', 'text/css')
        .html('.glyphicon-spin { animation: spin 1s infinite linear; } @keyframes spin { from { transform: rotate(0deg); } to { transform: rotate(360deg); } }')
        .appendTo('head');
});
