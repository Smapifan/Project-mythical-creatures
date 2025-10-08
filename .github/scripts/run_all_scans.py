import os
import json

# --- Setup directories ---
output_dir = "All_Code"
os.makedirs(output_dir, exist_ok=True)

# -------------------------
# 1️⃣ Linecount Scan
# -------------------------
print("=== Running Linecount Scan ===")
linecount_data = {}

for root, dirs, files in os.walk("."):
    for file in files:
        if file.endswith(".py"):
            filepath = os.path.join(root, file)
            with open(filepath, "r", encoding="utf-8") as f:
                lines = f.readlines()
                linecount_data[filepath] = len(lines)

# Save linecount JSON
linecount_json_path = os.path.join(output_dir, "linecount.json")
with open(linecount_json_path, "w", encoding="utf-8") as f:
    json.dump(linecount_data, f, indent=2)

print(f"Linecount finished. Saved in {linecount_json_path}")

# -------------------------
# 2️⃣ Generate Markdown for Linecount
# -------------------------
print("=== Generating Markdown for Linecount ===")
md_lines = ["# Code Linecount Report\n"]
for filepath, lines in linecount_data.items():
    md_lines.append(f"- `{filepath}`: {lines} lines")

filecode_md_path = "FILECODE.md"
with open(filecode_md_path, "w", encoding="utf-8") as f:
    f.write("\n".join(md_lines))

print(f"FILECODE.md created successfully!")

# -------------------------
# 3️⃣ Filetype Scan
# -------------------------
print("=== Running Filetype Scan ===")
filetype_data = {}

for root, dirs, files in os.walk("."):
    for file in files:
        ext = os.path.splitext(file)[1].lower() or "no_extension"
        filepath = os.path.join(root, file)
        filetype_data.setdefault(ext, []).append(filepath)

# Save filetype JSON
filetype_json_path = os.path.join(output_dir, "filetype.json")
with open(filetype_json_path, "w", encoding="utf-8") as f:
    json.dump(filetype_data, f, indent=2)

print(f"Filetype scan finished. Saved in {filetype_json_path}")

# -------------------------
# 4️⃣ Generate Markdown for Filetype
# -------------------------
print("=== Generating Markdown for Filetype ===")
md_lines = ["# Filetype Report\n"]
for ext, files in filetype_data.items():
    md_lines.append(f"## {ext} ({len(files)} files)")
    for file in files:
        md_lines.append(f"- `{file}`")

all_file_report_md_path = "ALL_FILE_REPORT.md"
with open(all_file_report_md_path, "w", encoding="utf-8") as f:
    f.write("\n".join(md_lines))

print(f"ALL_FILE_REPORT.md created successfully!")

print("=== All scans and markdown generation finished ===")
