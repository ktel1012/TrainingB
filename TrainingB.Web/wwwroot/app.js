// PWA Install
let deferredPrompt;

window.addEventListener('beforeinstallprompt', (e) => {
    e.preventDefault();
    deferredPrompt = e;
    document.getElementById('installBtn').style.display = 'block';
});

document.getElementById('installBtn').addEventListener('click', async () => {
    if (deferredPrompt) {
        deferredPrompt.prompt();
        const { outcome } = await deferredPrompt.userChoice;
        console.log(`User response: ${outcome}`);
        deferredPrompt = null;
        document.getElementById('installBtn').style.display = 'none';
    }
});

// Confirmation Dialog State
let pendingAction = null;

// Show confirmation dialog
function showConfirmDialog(title, message, callback) {
    document.getElementById('dialogTitle').textContent = title;
    document.getElementById('dialogMessage').textContent = message;
    document.getElementById('confirmDialog').classList.add('active');
    pendingAction = callback;
}

// Confirm dialog
function confirmDialog() {
    document.getElementById('confirmDialog').classList.remove('active');
    if (pendingAction) {
        pendingAction();
        pendingAction = null;
    }
}

// Cancel dialog
function cancelDialog() {
    document.getElementById('confirmDialog').classList.remove('active');
    pendingAction = null;
}

// Load scrapers
async function loadScrapers() {
    try {
        const response = await fetch('/api/scraper/scrapers');
        const scrapers = await response.json();

        const grid = document.getElementById('scraperGrid');
        grid.innerHTML = scrapers.map(s => `
            <button class="scraper-btn" onclick="confirmRunScraper('${s.name}', '${s.description}')">
                ${s.description}
            </button>
        `).join('');
    } catch (error) {
        showStatus('Error loading scrapers: ' + error.message, 'error');
    }
}

// Show status
function showStatus(message, type) {
    const status = document.getElementById('status');
    status.textContent = message;
    status.className = `status ${type}`;
    status.style.display = 'block';
}

// Hide status
function hideStatus() {
    document.getElementById('status').style.display = 'none';
}

// Confirm before running scraper
function confirmRunScraper(name, description) {
    showConfirmDialog(
        '🤖 Xác nhận chạy Scraper',
        `Bạn có chắc muốn chạy "${description}"?\n\nQuá trình có thể mất 30 giây - 2 phút.`,
        () => runScraper(name)
    );
}

// Run single scraper
async function runScraper(name) {
    const resultsDiv = document.getElementById('results');
    const buttons = document.querySelectorAll('.scraper-btn');

    buttons.forEach(btn => btn.disabled = true);
    showStatus(`🔄 Đang chạy ${name}... (có thể mất vài phút)`, 'loading');

    try {
        // Create abort controller with timeout (longer than server timeout)
        // 4D scrapers: 15min client timeout (server 10min, actual ~6-12min)
        // Others: 6min client timeout (server 5min)
        const is4D = name.includes('4D');
        const clientTimeoutMinutes = is4D ? 15 : 6;
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), clientTimeoutMinutes * 60 * 1000);

        const response = await fetch(`/api/scraper/run/${name}`, {
            method: 'POST',
            signal: controller.signal,
            // Prevent browser caching
            headers: {
                'Cache-Control': 'no-cache'
            }
        });

        clearTimeout(timeoutId);

        const data = await response.json();

        // Debug logging
        console.log('Response data:', data);
        console.log('Result text:', data.result);
        console.log('Response status:', response.status);

        // Handle different response statuses
        if (response.ok || data.success) {
            // Success case (200)
            showStatus(`✅ ${name} hoàn thành!`, 'success');

            // Display result
            const timestamp = new Date().toLocaleTimeString();
            const resultText = data.result || 'Không có thay đổi (No changes detected)';

            // Clear default text on first result
            if (resultsDiv.textContent === 'Chưa có kết quả...') {
                resultsDiv.textContent = '';
            }

            // Prepend new result
            const newResult = `[${timestamp}] ${name}:\n${resultText}\n\n`;
            resultsDiv.textContent = newResult + resultsDiv.textContent;

            console.log('Updated results div:', resultsDiv.textContent);
        } else if (response.status === 409) {
            // Conflict - another scraper is running
            showStatus(`⏳ ${name}: Server đang bận`, 'error');
            const message = data.message || `Scraper khác đang chạy: ${data.currentScraper || 'unknown'}`;
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ${name}: ${message}\n\n` + resultsDiv.textContent;
        } else if (response.status === 408) {
            // Timeout from server
            const timeoutMsg = is4D ? 'quá 12 phút' : 'quá 5 phút';
            showStatus(`⏱️ ${name} timeout (${timeoutMsg})`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ${name}: TIMEOUT - Scraper chạy quá lâu\n\n` + resultsDiv.textContent;
        } else {
            showStatus(`❌ Lỗi: ${data.error}`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ERROR ${name}: ${data.error}\n\n` + resultsDiv.textContent;
        }
    } catch (error) {
        if (error.name === 'AbortError') {
            const is4D = name.includes('4D');
            const timeoutMsg = is4D ? 'quá 15 phút' : 'quá 6 phút';
            showStatus(`⏱️ ${name} timeout (${timeoutMsg})`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ${name}: CLIENT TIMEOUT - Không nhận được response\n\n` + resultsDiv.textContent;
        } else {
            showStatus(`❌ Lỗi kết nối: ${error.message}`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ERROR ${name}: ${error.message}\n\n` + resultsDiv.textContent;
        }
    } finally {
        buttons.forEach(btn => btn.disabled = false);
        setTimeout(hideStatus, 5000); // Show status a bit longer
    }
}

// Confirm before running all scrapers
function confirmRunAll() {
    showConfirmDialog(
        '🚀 Xác nhận chạy TẤT CẢ',
        `Bạn có chắc muốn chạy TẤT CẢ 13 scrapers?\n\n⚠️ Quá trình sẽ mất 5-10 phút!\n\nBạn có thể tắt màn hình nhưng đừng tắt app.`,
        () => runAll()
    );
}

// Run all scrapers
async function runAll() {
    const resultsDiv = document.getElementById('results');
    const buttons = document.querySelectorAll('.scraper-btn');
    
    buttons.forEach(btn => btn.disabled = true);
    showStatus('Đang chạy tất cả scrapers...', 'loading');
    resultsDiv.textContent = 'Bắt đầu chạy tất cả scrapers...\n\n';
    
    try {
        const response = await fetch('/api/scraper/run-all', {
            method: 'POST'
        });
        
        const data = await response.json();
        
        if (response.ok) {
            showStatus(`✅ Hoàn thành ${data.results.length} scrapers!`, 'success');
            
            let output = `[${new Date().toLocaleTimeString()}] KẾT QUẢ TẤT CẢ:\n\n`;
            data.results.forEach(r => {
                if (r.success) {
                    output += `✅ ${r.scraper}:\n${r.result}\n\n`;
                } else {
                    output += `❌ ${r.scraper}: ${r.error}\n\n`;
                }
            });
            
            resultsDiv.textContent = output;
        } else {
            showStatus(`❌ Lỗi: ${data.error}`, 'error');
        }
    } catch (error) {
        showStatus(`❌ Lỗi: ${error.message}`, 'error');
        resultsDiv.textContent = `ERROR: ${error.message}\n\n` + resultsDiv.textContent;
    } finally {
        buttons.forEach(btn => btn.disabled = false);
        setTimeout(hideStatus, 5000);
    }
}

// Check API status on load
async function checkStatus() {
    try {
        const response = await fetch('/api/scraper/status');
        const data = await response.json();
        console.log('API Status:', data);
    } catch (error) {
        showStatus('⚠️ Cannot connect to API', 'error');
    }
}

// Initialize
window.addEventListener('load', () => {
    checkStatus();
    loadScrapers();
    
    // Register service worker
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js')
            .then(() => console.log('Service Worker registered'))
            .catch(err => console.log('Service Worker registration failed:', err));
    }
});

// Copy results to clipboard
function copyResults() {
    const results = document.getElementById('results').textContent;

    if (results === 'Chưa có kết quả...') {
        showStatus('Chưa có kết quả để copy!', 'error');
        setTimeout(hideStatus, 2000);
        return;
    }

    // Modern clipboard API
    if (navigator.clipboard && navigator.clipboard.writeText) {
        navigator.clipboard.writeText(results)
            .then(() => {
                showStatus('✅ Đã copy kết quả!', 'success');
                setTimeout(hideStatus, 2000);
            })
            .catch(err => {
                console.error('Copy failed:', err);
                fallbackCopy(results);
            });
    } else {
        fallbackCopy(results);
    }
}

// Fallback copy method for older browsers
function fallbackCopy(text) {
    const textarea = document.createElement('textarea');
    textarea.value = text;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';
    document.body.appendChild(textarea);
    textarea.select();

    try {
        document.execCommand('copy');
        showStatus('✅ Đã copy kết quả!', 'success');
        setTimeout(hideStatus, 2000);
    } catch (err) {
        showStatus('❌ Không thể copy, hãy long-press để select!', 'error');
        setTimeout(hideStatus, 3000);
    }

    document.body.removeChild(textarea);
}
