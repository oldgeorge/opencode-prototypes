// Simple tabbed navigation for OG.Admin prototype
(() => {
  const tabsContainer = document.getElementById('tabs');
  const frame = document.getElementById('contentFrame');
  // Ensure this script only runs on pages that have the tabs container
  if (!tabsContainer || !frame) return;

  const openTabs = new Map(); // src -> tabElement
  function createTab(title, src) {
    // If tab already exists, activate it
    if (openTabs.has(src)) {
      activateTab(openTabs.get(src));
      return;
    }
    const tab = document.createElement('div');
    tab.className = 'tab active';
    tab.dataset.src = src;
    tab.style.userSelect = 'none';
    tab.innerHTML = `<span>${title}</span><span class="close" title="Close tab">×</span>`;
    // Add click to activate if clicking on the main area (not the close)
    tab.addEventListener('click', (e) => {
      // If clicked on close, handled by separate listener
      if (e.target.classList.contains('close')) return;
      activateTab(tab);
    });
    // Close handler
    tab.querySelector('.close').addEventListener('click', (e) => {
      e.stopPropagation();
      closeTab(tab);
    });
    // Insert before any remaining tabs
    tabsContainer.appendChild(tab);
    // Mark as active and load content
    setActiveVisual(tab);
    frame.src = src;
    openTabs.set(src, tab);
  }

  function setActiveVisual(tab) {
    // Remove active from all siblings
    tabsContainer.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    tab.classList.add('active');
  }

  function activateTab(tab) {
    if (!tab) return;
    // Ensure this tab is active and others not
    setActiveVisual(tab);
    frame.src = tab.dataset.src;
  }

  function closeTab(tab) {
    if (!tab) return;
    const wasActive = tab.classList.contains('active');
    const src = tab.dataset.src;
    openTabs.delete(src);
    tab.remove();
    // If closed tab was active, switch to the last tab or default dashboard
    if (wasActive) {
      const last = tabsContainer.querySelector('.tab:last-child');
      if (last) {
        activateTab(last);
      } else {
        // Fallback to dashboard
        frame.src = 'dashboard.html';
      }
    }
  }

  // Wire up admin tree to create tabs
  function wireTreeLinks() {
    const tree = document.getElementById('treeMenu');
    if (!tree) return;
    tree.addEventListener('click', (e) => {
      const item = e.target.closest('.tree-item');
      if (!item) return;
      const src = item.dataset.src;
      const label = item.textContent.trim();
      if (src) {
        createTab(label, src);
      }
    });
  }

  // Initialize
  document.addEventListener('DOMContentLoaded', () => {
    //Create initial Dashboard tab
    // Use a separate function so that on first load, there is a tab
    createTab('📊 工作台', 'dashboard.html');
    wireTreeLinks();
  });
})();
