var blazor_progress = 0;
window.loadResourceCallback = (total, name, response) => {
    if (name.endsWith('.dll')) {
        blazor_progress++;
        const value = parseInt((blazor_progress * 100.0) / total);
        const pct = value + '%';

        const progressbar = document.getElementsByClassName('progressbar');
        for (const bar of progressbar) {
            bar.style.width = pct;
        }
    }
}