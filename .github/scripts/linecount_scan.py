import os
import json
import subprocess
from pathlib import Path
import tempfile
import shutil

# --- Funktionen ---
def count_lines(file_path):
    """Zählt die Zeilen einer Datei, ignoriert Fehler."""
    try:
        with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
            return sum(1 for _ in f)
    except Exception:
        return 0

# --- Einstellungen ---
blacklist_exts = {".zip", ".exe", ".dll", ".png", ".jpg", ".jpeg", ".gif", ".bmp"}
output_dir = Path("All_Code")
output_dir.mkdir(exist_ok=True)

# --- Branches holen ---
branches = subprocess.check_output(["git", "branch", "-r"]).decode().splitlines()
branches = [b.strip().replace("origin/", "") for b in branches if "origin/" in b and "HEAD" not in b]

# --- Branches scannen ---
for branch in branches:
    print(f"Scanne Branch: {branch}")

    # Temporärer Worktree
    tmpdir = tempfile.mkdtemp(prefix="worktree_")
    try:
        subprocess.run(["git", "fetch", "origin", branch], check=True)
        subprocess.run(["git", "worktree", "add", tmpdir, f"origin/{branch}"], check=True)

        total_lines = 0
        ext_counter = {}

        for root, dirs, files in os.walk(tmpdir):
            if ".git" in root:
                continue
            for f in files:
                ext = os.path.splitext(f)[1].lower() if "." in f else "(no ext)"
                if ext in blacklist_exts:
                    ext = "other"
                path = os.path.join(root, f)
                lines = count_lines(path)
                total_lines += lines
                ext_counter[ext] = ext_counter.get(ext, 0) + lines

        # JSON speichern
        result = {
            "branch": branch,
            "total_lines": total_lines,
            "lines_by_ext": ext_counter
        }

        json_path = output_dir / f"Lines_{branch}.json"
        with open(json_path, "w", encoding="utf-8") as jf:
            json.dump(result, jf, indent=2, ensure_ascii=False)

        print(f"Branch '{branch}' gescannt. JSON: {json_path}")

    finally:
        # Worktree wieder entfernen
        subprocess.run(["git", "worktree", "remove", tmpdir, "--force"], check=True)
        shutil.rmtree(tmpdir, ignore_errors=True)

print("Fertig! Alle Branches gescannt.")
