import os
import json
from pathlib import Path

# Arbeitsverzeichnis: Repo-Root
repo_root = Path.cwd()  # Läuft im Root, kein extra Pfad nötig
input_dir = repo_root / "All_Code"
output_file = repo_root / "FILECODE.md"

# Alle JSON-Dateien sammeln
json_files = sorted(input_dir.glob("*.json"))
if not json_files:
    print(f"Keine JSON-Dateien in {input_dir} gefunden!")
else:
    rows = []

    for file in json_files:
        with open(file, "r", encoding="utf-8") as f:
            data = json.load(f)
        branch = data.get("branch", "unknown")
        total = data.get("total_lines", 0)
        by_ext = data.get("lines_by_ext", {})

        rows.append(f"### Branch `{branch}`\n")
        rows.append("| Dateityp | Zeilen | Prozent |\n")
        rows.append("|----------|--------|---------|\n")
        for ext, count in sorted(by_ext.items(), key=lambda x: -x[1]):
            perc = (count / total) * 100 if total else 0
            rows.append(f"| `{ext}` | {count} | {perc:.2f}% |\n")
        rows.append(f"| **Total** | **{total}** | 100% |\n\n")

    # Markdown schreiben
    with open(output_file, "w", encoding="utf-8") as f:
        f.write("## Code Line Counts (all Branches)\n\n")
        f.writelines(rows)

    print(f"Markdown erstellt: {output_file}")
