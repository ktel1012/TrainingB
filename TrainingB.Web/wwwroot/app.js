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

// Load scrapers
async function loadScrapers() {
    try {
        const response = await fetch('/api/scraper/scrapers');
        const scrapers = await response.json();
        
        const grid = document.getElementById('scraperGrid');
        grid.innerHTML = scrapers.map(s => `
            <button class="scraper-btn" onclick="runScraper('${s.name}')">
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

// Run single scraper
async function runScraper(name) {
    const resultsDiv = document.getElementById('results');
    const buttons = document.querySelectorAll('.scraper-btn');

    buttons.forEach(btn => btn.disabled = true);
    showStatus(`🔄 Đang chạy ${name}... (có thể mất vài phút)`, 'loading');

    try {
        // Create abort controller with 6-minute timeout (longer than server's 5min)
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 6 * 60 * 1000);

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

        if (response.ok) {
            showStatus(`✅ ${name} hoàn thành!`, 'success');

            // Display result
            const timestamp = new Date().toLocaleTimeString();
            const resultText = data.result || 'No changes detected';

            // Clear default text on first result
            if (resultsDiv.textContent === 'Chưa có kết quả...') {
                resultsDiv.textContent = '';
            }

            // Prepend new result
            const newResult = `[${timestamp}] ${name}:\n${resultText}\n\n`;
            resultsDiv.textContent = newResult + resultsDiv.textContent;

            console.log('Updated results div:', resultsDiv.textContent);
        } else if (response.status === 408) {
            // Timeout from server
            showStatus(`⏱️ ${name} timeout (quá 5 phút)`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ${name}: TIMEOUT - Scraper chạy quá lâu\n\n` + resultsDiv.textContent;
        } else {
            showStatus(`❌ Lỗi: ${data.error}`, 'error');
            resultsDiv.textContent = `[${new Date().toLocaleTimeString()}] ERROR ${name}: ${data.error}\n\n` + resultsDiv.textContent;
        }
    } catch (error) {
        if (error.name === 'AbortError') {
            showStatus(`⏱️ ${name} timeout (quá 6 phút)`, 'error');
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
